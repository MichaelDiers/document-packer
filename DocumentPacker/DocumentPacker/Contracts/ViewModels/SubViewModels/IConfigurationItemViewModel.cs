namespace DocumentPacker.Contracts.ViewModels.SubViewModels;

/// <summary>
///     Describes a configuration item.
/// </summary>
public interface IConfigurationItemViewModel
{
    /// <summary>
    ///     Gets or sets the type of the configuration item.
    /// </summary>
    ConfigurationItemType ConfigurationItemType { get; set; }

    /// <summary>
    ///     Gets or sets the configuration value.
    /// </summary>
    string? Name { get; set; }
}
