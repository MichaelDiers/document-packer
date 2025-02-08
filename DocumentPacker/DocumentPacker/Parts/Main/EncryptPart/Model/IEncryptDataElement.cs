namespace DocumentPacker.Parts.Main.EncryptPart.Model;

/// <summary>
///     Describes an element that gets encrypted.
/// </summary>
public interface IEncryptDataElement
{
    /// <summary>
    ///     Gets the description of the element.
    /// </summary>
    string Description { get; }

    /// <summary>
    ///     Gets the type of the encrypt item.
    /// </summary>
    EncryptItemType EncryptItemType { get; }

    /// <summary>
    ///     Gets the value indicating whether the item is required or optional.
    /// </summary>
    bool IsRequired { get; }

    /// <summary>
    ///     Gets the value of the encrypt item.
    /// </summary>
    string Value { get; }
}
