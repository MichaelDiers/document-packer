namespace DocumentPacker.EventHandling;

public interface IHandleBackLink
{
    void HandleBackLink(object? sender, BackLinkEventArgs eventArgs);
}
