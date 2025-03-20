namespace DocumentPacker.Tests.Commands;

using DocumentPacker.Commands;
using Libs.Wpf.Controls.CustomMessageBox;
using Moq;

public class CommandExecutorTests
{
    [Theory]
    [InlineData(
        false,
        false,
        false,
        "error",
        true,
        false,
        null)]
    [InlineData(
        true,
        false,
        false,
        "error",
        true,
        false,
        "Ooooops...an unknown error occured.")]
    [InlineData(
        true,
        true,
        false,
        "error",
        false,
        false,
        "error")]
    [InlineData(
        true,
        true,
        true,
        "success",
        false,
        true,
        "success")]
    [InlineData(
        true,
        true,
        true,
        "success",
        true,
        false,
        "Ooooops...an unknown error occured.\r\n\r\nThe method or operation is not implemented.")]
    public async Task ExecuteTests(
        bool validateResult,
        bool commandSyncResult,
        bool executeSuccess,
        string? executeMessage,
        bool executeThrowsException,
        bool resultSuccess,
        string? resultMessage
    )
    {
        var commandSyncMock = new Mock<ICommandSync>();
        commandSyncMock.Setup(
                commandSync => commandSync.Enter(
                    false,
                    null))
            .Returns(commandSyncResult)
            .Verifiable(!validateResult ? Times.Never : Times.Once);
        commandSyncMock.Setup(commandSync => commandSync.Exit())
            .Verifiable(!validateResult || !commandSyncResult ? Times.Never : Times.Once);

        var actual = await CommandExecutor.Execute(
            () => validateResult,
            commandSyncMock.Object,
            () => executeThrowsException
                ? throw new NotImplementedException()
                : Task.FromResult((executeSuccess, executeMessage)),
            null);

        Assert.Equal(
            resultSuccess,
            actual.success);
        Assert.Equal(
            resultMessage,
            actual.message);

        commandSyncMock.VerifyAll();
        commandSyncMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(
        false,
        null,
        false,
        "",
        "",
        MessageBoxImage.None,
        false)]
    [InlineData(
        false,
        "",
        false,
        "",
        "",
        MessageBoxImage.None,
        false)]
    [InlineData(
        false,
        "error",
        false,
        "error",
        "Error",
        MessageBoxImage.Error,
        true)]
    [InlineData(
        true,
        "success",
        false,
        "success",
        "",
        MessageBoxImage.Information,
        true)]
    [InlineData(
        true,
        "success",
        true,
        "One or more errors occurred. (The method or operation is not implemented.)",
        "",
        MessageBoxImage.Error,
        true)]
    public void PostExecute(
        bool taskSuccess,
        string? taskMessage,
        bool taskThrowsException,
        string messageBoxMessage,
        string messageBoxCaption,
        MessageBoxImage messageBoxImage,
        bool called
    )
    {
        var messageBoxServiceMock = new Mock<IMessageBoxService>();
        messageBoxServiceMock.Setup(
                messageBoxService => messageBoxService.Show(
                    messageBoxMessage,
                    messageBoxCaption,
                    MessageBoxButtons.Ok,
                    MessageBoxButtons.Ok,
                    messageBoxImage,
                    null,
                    null))
            .Returns(MessageBoxButtons.Ok)
            .Verifiable(!called ? Times.Never : Times.Once);

        var task = Task.Factory.StartNew(
            () => !taskThrowsException ? (taskSuccess, taskMessage) : throw new NotImplementedException());

        CommandExecutor.PostExecute(
            task,
            messageBoxServiceMock.Object);

        messageBoxServiceMock.VerifyAll();
        messageBoxServiceMock.VerifyNoOtherCalls();
    }
}
