namespace DocumentPacker.ViewModels;

using System.Reflection;
using System.Windows.Input;
using DocumentPacker.Contracts.ViewModels;

/// <summary>
///     Describes the main view model.
/// </summary>
internal class MainViewModel : BaseViewModel, IMainViewModel
{
    /// <summary>
    ///     The fatal error message text.
    /// </summary>
    private string? fatalErrorMessage;

    private bool runExecuted1;
    private bool runExecuted2;
    private bool runExecuted3;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MainViewModel" /> class.
    /// </summary>
    public MainViewModel()
    {
        TaskCommand.FatalError += (sender, s) => this.FatalErrorMessage = s;
    }

    /// <summary>
    ///     Gets the close fatal error message command.
    /// </summary>
    public ICommand CloseFatalErrorMessageCommand =>
        new TaskCommand(
            _ => this.FatalErrorMessage is not null,
            (_, _) =>
            {
                this.FatalErrorMessage = null;
                return Task.CompletedTask;
            });

    /// <summary>
    ///     Gets or sets the fatal error message text.
    /// </summary>
    public string? FatalErrorMessage
    {
        get => this.fatalErrorMessage;
        set =>
            this.SetField(
                ref this.fatalErrorMessage,
                value);
    }

    public ICommand RunCommand1 =>
        new TaskCommand(
            _ => !this.RunExecuted1,
            async (_, cancellationToken) =>
            {
                await Task.Delay(
                    40,
                    cancellationToken);
                throw new AggregateException(
                    new AccessViolationException("foo."),
                    new AbandonedMutexException("bar."),
                    new AmbiguousMatchException());
                this.RunExecuted1 = true;
            });

    public ICommand RunCommand2 =>
        new TaskCommand(
            _ => !this.RunExecuted2,
            async (_, cancellationToken) =>
            {
                await Task.Delay(
                    4000,
                    cancellationToken);

                this.RunExecuted2 = true;
            });

    public ICommand RunCommand3 =>
        new TaskCommand(
            _ => !this.RunExecuted3 && this.RunExecuted1 && this.RunExecuted2,
            async (_, cancellationToken) =>
            {
                await Task.Delay(
                    4000,
                    cancellationToken);
                this.RunExecuted3 = true;
            });

    public bool RunExecuted1
    {
        get => this.runExecuted1;
        set =>
            this.SetField(
                ref this.runExecuted1,
                value);
    }

    public bool RunExecuted2
    {
        get => this.runExecuted2;
        set =>
            this.SetField(
                ref this.runExecuted2,
                value);
    }

    public bool RunExecuted3
    {
        get => this.runExecuted3;
        set =>
            this.SetField(
                ref this.runExecuted3,
                value);
    }
}
