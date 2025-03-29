namespace DocumentPacker.Parts.SubParts.LoadConfigurationSubPart;

using DocumentPacker.Models;

/// <summary>
///     Describes the <see cref="EventArgs" /> of the <see cref="LoadConfigurationViewModel.ConfigurationLoaded" /> event.
/// </summary>
/// <param name="configurationModel">The data of the <see cref="EventArgs" />.</param>
public class LoadConfigurationEventArgs(ConfigurationModel configurationModel) : EventArgs
{
    /// <summary>
    ///     Gets the data of the <see cref="EventArgs" />.
    /// </summary>
    public ConfigurationModel ConfigurationModel { get; } = configurationModel;
}
