namespace DocumentPacker.ViewModels;

using System.Windows.Input;
using DocumentPacker.Contracts.ViewModels;

/// <summary>
///     Describes the main view model.
/// </summary>
internal class MainViewModel : BaseViewModel, IMainViewModel
{
    private bool runExecuted1;
    private bool runExecuted2;
    private bool runExecuted3;

    public ICommand RunCommand1 =>
        new TaskCommand(
            _ => !this.RunExecuted1,
            (_, cancellationToken) =>
            {
                for (var i = 0; i < 5; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    Thread.Sleep(1000);
                }

                this.RunExecuted1 = true;
                return Task.CompletedTask;
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
