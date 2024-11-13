namespace DocumentPacker.Tests.ViewModels;

using DocumentPacker.Contracts.ViewModels;
using DocumentPacker.ViewModels;
using Moq;

/// <summary>
///     Tests for <see cref="TaskCommand" />.
/// </summary>
/// <seealso cref="System.IDisposable" />
public class TaskCommandTests : IDisposable
{
    private const int DefaultDelay = 50;
    private readonly Mock<IDispatcher> dispatcherMock = new();

    public TaskCommandTests()
    {
        this.dispatcherMock.Setup(dispatcher => dispatcher.CheckAccess()).Returns(true);
        this.dispatcherMock.Setup(dispatcher => dispatcher.Invoke(It.IsAny<Action>()))
            .Callback<Action>(callback => callback());

        Assert.False(TaskCommand.IsExecutingCommands);
    }

    public void Dispose()
    {
        for (var i = 0; i < 10 && TaskCommand.IsExecutingCommands; ++i)
        {
            Thread.Sleep(TaskCommandTests.DefaultDelay);
        }

        Assert.False(TaskCommand.IsExecutingCommands);
    }

    [Fact]
    public void CancelCommandChangedIsRaisedIfCommandIsExecuting()
    {
        var command = this.CreateCommand(
            _ => true,
            async (_, _) => await Task.Delay(
                TaskCommandTests.DefaultDelay * 5,
                CancellationToken.None));
        var eventIsRaised = false;
        TaskCommand.CancelCommandChanged += (_, _) => eventIsRaised = true;

        command.Execute(null);
        Assert.True(eventIsRaised);
    }

    [Fact]
    public async Task CancelCommandExecuteCancelsExecutingCommands()
    {
        const int delay = 1000;
        var commandTerminated = false;
        var command = this.CreateCommand(
            _ => true,
            async (_, cancellationToken) =>
            {
                await Task.Delay(
                    delay,
                    cancellationToken);
                commandTerminated = true;
            });

        Assert.False(TaskCommand.IsExecutingCommands);

        command.Execute(null);

        Assert.True(TaskCommand.IsExecutingCommands);

        Assert.NotNull(TaskCommand.CancelCommand);
        Assert.True(TaskCommand.CancelCommand.CanExecute(null));
        TaskCommand.CancelCommand.Execute(null);

        await this.WaitForCommandTermination();

        Assert.False(TaskCommand.IsExecutingCommands);
        Assert.False(commandTerminated);
    }

    [Fact]
    public void CancelCommandIsNotNullIfCommandExecutes()
    {
        var command = this.CreateCommand(
            _ => true,
            async (_, _) => await Task.Delay(
                TaskCommandTests.DefaultDelay,
                CancellationToken.None));

        command.Execute(null);

        Assert.NotNull(TaskCommand.CancelCommand);
    }

    [Fact]
    public void CancelCommandIsNullIfNoCommandExecutes()
    {
        Assert.False(TaskCommand.IsExecutingCommands);
        Assert.Null(TaskCommand.CancelCommand);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanExecute(bool expectedResult)
    {
        var command = this.CreateCommand(
            _ => expectedResult,
            (_, _) => Task.CompletedTask);

        Assert.Equal(
            expectedResult,
            command.CanExecute(expectedResult));
    }

    [Fact]
    public void CanExecuteReturnsFalseIfACommandIsExecuting()
    {
        var executingCommand = this.CreateCommand(
            _ => true,
            async (_, _) => await Task.Delay(
                TaskCommandTests.DefaultDelay,
                CancellationToken.None));
        var requestingCommand = this.CreateCommand(
            _ => true,
            (_, _) => Task.CompletedTask);

        executingCommand.Execute(null);
        Assert.True(TaskCommand.IsExecutingCommands);

        Assert.False(requestingCommand.CanExecute(null));
        Assert.True(TaskCommand.IsExecutingCommands);
    }

    [Fact]
    public async Task FatalErrorDoesNotOccurIfBackgroundTaskTerminates()
    {
        var command = this.CreateCommand(
            _ => true,
            (_, _) => Task.CompletedTask);

        string? actualError = null;
        TaskCommand.FatalError += (_, message) => { actualError = message; };

        command.Execute(null);
        await this.WaitForCommandTermination();

        Assert.Null(actualError);
    }

    [Fact]
    public async Task FatalErrorOccursIfBackgroundTaskThrowsAnError()
    {
        const string expectedError = nameof(expectedError);
        var command = this.CreateCommand(
            _ => true,
            (_, _) => throw new Exception(expectedError));

        string? actualError = null;
        TaskCommand.FatalError += (_, message) => { actualError = message; };

        command.Execute(null);
        await this.WaitForCommandTermination();

        Assert.NotNull(actualError);
        Assert.Equal(
            expectedError,
            actualError);
    }

    [Fact]
    public async Task IsExecutingCommandsChangedIsRaisedIfCommandIsExecuting()
    {
        var command = this.CreateCommand(
            _ => true,
            (_, _) => Task.CompletedTask);
        var eventRaised = 0;
        TaskCommand.IsExecutingCommandsChanged += (_, _) => eventRaised++;

        command.Execute(null);

        await this.WaitForCommandTermination();

        Assert.Equal(
            2,
            eventRaised);
    }

    [Fact]
    public async Task IsExecutingCommandsCheckForMultipleCommands()
    {
        const int shortDelay = 50;
        const int longDelay = 2 * shortDelay;

        var executingCommandShort = this.CreateCommand(
            _ => true,
            async (_, _) => await Task.Delay(
                shortDelay,
                CancellationToken.None));
        var executingCommandLong = this.CreateCommand(
            _ => true,
            async (_, _) => await Task.Delay(
                longDelay,
                CancellationToken.None));

        executingCommandShort.Execute(null);
        executingCommandLong.Execute(null);

        Assert.True(TaskCommand.IsExecutingCommands);

        await Task.Delay(shortDelay);
        Assert.True(TaskCommand.IsExecutingCommands);
        await Task.Delay(longDelay);
        Assert.False(TaskCommand.IsExecutingCommands);
    }

    [Fact]
    public async Task IsExecutingCommandsIsFalseAfterCommandExecution()
    {
        var executingCommand = this.CreateCommand(
            _ => true,
            async (_, _) => await Task.Delay(
                TaskCommandTests.DefaultDelay,
                CancellationToken.None));

        executingCommand.Execute(null);
        Assert.True(TaskCommand.IsExecutingCommands);
        await Task.Delay(TaskCommandTests.DefaultDelay * 2);

        Assert.False(TaskCommand.IsExecutingCommands);
    }

    [Fact]
    public void IsExecutingCommandsIsFalseIfNoCommandIsExecuting()
    {
        Assert.False(TaskCommand.IsExecutingCommands);
    }

    [Fact]
    public void IsExecutingCommandsIsTrueIfCommandIsExecuting()
    {
        var executingCommand = this.CreateCommand(
            _ => true,
            async (_, _) => await Task.Delay(
                TaskCommandTests.DefaultDelay,
                CancellationToken.None));

        executingCommand.Execute(null);

        Assert.True(TaskCommand.IsExecutingCommands);
    }

    private TaskCommand CreateCommand(Func<object?, bool> canExecute, Func<object?, CancellationToken, Task> execute)
    {
        return new TaskCommand(
            canExecute,
            execute,
            false,
            this.dispatcherMock.Object);
    }

    private async Task WaitForCommandTermination(int delay = TaskCommandTests.DefaultDelay)
    {
        for (var i = 0; i < 100 && TaskCommand.IsExecutingCommands; ++i)
        {
            await Task.Delay(delay);
        }

        Assert.False(TaskCommand.IsExecutingCommands);
    }
}
