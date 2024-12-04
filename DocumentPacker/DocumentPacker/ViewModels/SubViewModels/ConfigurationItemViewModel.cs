namespace DocumentPacker.ViewModels.SubViewModels;

using DocumentPacker.Contracts;

/// <summary>
///     Describes a configuration item.
/// </summary>
/// <seealso cref="DocumentPacker.ViewModels.BaseViewModel" />
internal class ConfigurationItemViewModel : BaseViewModel
{
    /// <summary>
    ///     The configuration item type.
    /// </summary>
    private ConfigurationItemType configurationItemType;

    /// <summary>
    ///     The value of the item.
    /// </summary>
    private string? value;

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
    ///     Gets or sets the configuration value.
    /// </summary>
    public string? Value
    {
        get => this.value;
        set =>
            this.SetField(
                ref this.value,
                value);
    }
}
