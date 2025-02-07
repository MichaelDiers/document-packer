namespace DocumentPacker.EventHandling;

public interface IEventHandlerCenter
{
    event EventHandler? Closed;
    void Initialize(IServiceProvider serviceProvider);
}
