namespace DocumentPacker.Mvvm;

using DocumentPacker.EventHandling;
using Libs.Wpf.ViewModels;

internal class ApplicationBaseViewModel : ViewModelBase, IApplicationViewModel
{
    public event EventHandler<BackLinkEventArgs>? BackLinkCreated;

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public virtual void Dispose()
    {
        this.BackLinkCreated = null;
        this.ShowViewRequested = null;
    }

    public virtual void HandleShowViewRequested(object? sender, ShowViewRequestedEventArgs eventArgs)
    {
    }

    public event EventHandler<ShowViewRequestedEventArgs>? ShowViewRequested;

    protected void InvokeBackLinkCreated(object? sender, BackLinkEventArgs eventArgs)
    {
        this.BackLinkCreated?.Invoke(
            sender,
            eventArgs);
    }

    protected void InvokeShowViewRequested(object? sender, ShowViewRequestedEventArgs eventArgs)
    {
        this.ShowViewRequested?.Invoke(
            sender,
            eventArgs);
    }
}
