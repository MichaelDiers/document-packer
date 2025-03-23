namespace DocumentPacker.EventHandling;

public interface IApplicationViewModel : IDisposable
{
    public event EventHandler<BackLinkEventArgs> BackLinkCreated;
    public void HandleShowViewRequested(object? sender, ShowViewRequestedEventArgs eventArgs);

    public event EventHandler<ShowViewRequestedEventArgs> ShowViewRequested;
}
