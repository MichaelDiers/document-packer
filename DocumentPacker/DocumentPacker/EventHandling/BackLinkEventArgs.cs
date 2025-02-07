namespace DocumentPacker.EventHandling;

public class BackLinkEventArgs(ApplicationElementPart part, IApplicationView view) : EventArgs
{
    public ApplicationElementPart Part { get; } = part;

    public IApplicationView View { get; } = view;
}
