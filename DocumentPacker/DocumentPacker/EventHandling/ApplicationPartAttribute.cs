namespace DocumentPacker.EventHandling;

[AttributeUsage(
    AttributeTargets.Property,
    AllowMultiple = true)]
internal class ApplicationPartAttribute(ApplicationElementPart part) : Attribute
{
    public ApplicationElementPart Part { get; } = part;
}
