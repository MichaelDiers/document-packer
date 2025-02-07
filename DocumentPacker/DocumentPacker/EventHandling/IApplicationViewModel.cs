namespace DocumentPacker.EventHandling;

public interface IApplicationViewModel : IDisposable
{
    event EventHandler<BackLinkEventArgs> BackLinkCreated;
    void HandleShowViewRequested(object? sender, ShowViewRequestedEventArgs eventArgs);

    event EventHandler<ShowViewRequestedEventArgs> ShowViewRequested;
}
