namespace DocumentPacker.Models;

using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;

/// <summary>
///     Describes the data of a document packer configuration item.
/// </summary>
public class ConfigurationItemModel
{
    /// <summary>
    ///     Describes the data of a document packer configuration item.
    /// </summary>
    public ConfigurationItemModel(ConfigurationItemType configurationItemType, bool isRequired, string itemDescription)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(itemDescription);

        this.ConfigurationItemType = configurationItemType;
        this.IsRequired = isRequired;
        this.ItemDescription = itemDescription;
    }

    /// <summary>
    ///     Gets the type of the configuration item.
    /// </summary>
    public ConfigurationItemType ConfigurationItemType { get; }

    /// <summary>
    ///     Gets a value that specifies if the value is required.
    /// </summary>
    public bool IsRequired { get; }

    /// <summary>
    ///     Gets the item description.
    /// </summary>
    public string ItemDescription { get; }
}
