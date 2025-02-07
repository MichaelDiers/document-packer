namespace DocumentPacker.EventHandling;

public class ShowViewRequestedEventArgs(ApplicationElementPart part)
{
    public object? Data { get; set; }
    public ApplicationElementPart Part { get; } = part;

    public bool SuppressBackLink { get; set; } = false;

    public IApplicationView? View { get; set; }
}
