namespace DocumentPacker.Parts2.Main.EncryptPart.Model;

/// <summary>
///     Describes an element that gets encrypted.
/// </summary>
public class EncryptDataElement(
    string description,
    EncryptItemType encryptItemType,
    string value,
    bool isRequired
) : IEncryptDataElement
{
    /// <summary>
    ///     Gets the description of the element.
    /// </summary>
    public string Description { get; } = description;

    /// <summary>
    ///     Gets the type of the encrypt item.
    /// </summary>
    public EncryptItemType EncryptItemType { get; } = encryptItemType;

    /// <summary>
    ///     Gets the value indicating whether the item is required or optional.
    /// </summary>
    public bool IsRequired { get; } = isRequired;

    /// <summary>
    ///     Gets the value of the encrypt item.
    /// </summary>
    public string Value { get; } = value;
}
