namespace DocumentPacker.EventHandling;

[AttributeUsage(
    AttributeTargets.Class,
    Inherited = false)]
internal class DataContextAttribute(ApplicationElementPart part) : Attribute
{
    public ApplicationElementPart Part { get; } = part;
}
