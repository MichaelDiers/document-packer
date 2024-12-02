namespace DocumentPacker.ViewModels.SubViewModels;

using System.Windows.Input;
using DocumentPacker.Contracts;
using DocumentPacker.Contracts.ViewModels.SubViewModels;

/// <summary>
///     View model for the load configuration view.
/// </summary>
/// <seealso cref="DocumentPacker.ViewModels.SubViewModels.SubViewModel" />
/// <seealso cref="DocumentPacker.Contracts.ViewModels.SubViewModels.ILoadConfigurationViewModel" />
internal class LoadConfigurationViewModel() : SubViewModel(SubViewId.LoadConfiguration), ILoadConfigurationViewModel
{
    /// <summary>
    ///     The configuration file name.
    /// </summary>
    private string configurationFileName = string.Empty;

    /// <summary>
    ///     Gets or sets the name of the configuration file.
    /// </summary>
    public string ConfigurationFileName
    {
        get => this.configurationFileName;
        set =>
            this.SetField(
                ref this.configurationFileName,
                value);
    }

    /// <summary>
    ///     Gets the load configuration file command.
    /// </summary>
    public ICommand LoadConfigurationFileCommand { get; } = new TaskCommand(
        fileName => fileName is not null && !string.IsNullOrWhiteSpace(fileName as string),
        (fileName, cancellationToken) => Task.CompletedTask);
}
