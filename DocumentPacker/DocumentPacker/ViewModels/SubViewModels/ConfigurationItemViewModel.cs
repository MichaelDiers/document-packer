namespace DocumentPacker.ViewModels.SubViewModels;

using DocumentPacker.Contracts;
using DocumentPacker.Contracts.ViewModels.SubViewModels;

/// <summary>
///     Describes a configuration item.
/// </summary>
/// <seealso cref="DocumentPacker.ViewModels.BaseViewModel" />
internal class ConfigurationItemViewModel : BaseViewModel, IConfigurationItemViewModel
{
    /// <summary>
    ///     The configuration item type.
    /// </summary>
    private ConfigurationItemType configurationItemType;

    /// <summary>
    ///     The name of the item.
    /// </summary>
    private string? name;

    /// <summary>
    ///     Gets or sets the type of the configuration item.
    /// </summary>
    public ConfigurationItemType ConfigurationItemType
    {
        get => this.configurationItemType;
        set =>
            this.SetField(
                ref this.configurationItemType,
                value);
    }

    /// <summary>
    ///     Gets or sets the configuration name.
    /// </summary>
    public string? Name
    {
        get => this.name;
        set =>
            this.SetField(
                ref this.name,
                value);
    }
}
