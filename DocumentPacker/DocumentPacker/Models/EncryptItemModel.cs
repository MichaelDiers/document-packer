namespace DocumentPacker.Models;

using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;

public class EncryptItemModel(
    ConfigurationItemType configurationItemType,
    bool isRequired,
    string? value,
    string? id
)
{
    public ConfigurationItemType ConfigurationItemType { get; } = configurationItemType;
    public string? Id { get; } = id;
    public bool IsRequired { get; } = isRequired;
    public string? Value { get; } = value;
}
