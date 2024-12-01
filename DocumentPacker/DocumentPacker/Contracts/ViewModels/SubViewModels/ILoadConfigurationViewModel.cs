namespace DocumentPacker.Contracts.ViewModels.SubViewModels;

using System.Windows.Input;

/// <summary>
///     View model for the load configuration view.
/// </summary>
/// <seealso cref="DocumentPacker.Contracts.ViewModels.SubViewModels.ISubViewModel" />
public interface ILoadConfigurationViewModel : ISubViewModel
{
    /// <summary>
    ///     Gets or sets the name of the configuration file.
    /// </summary>
    string ConfigurationFileName { get; set; }

    /// <summary>
    ///     Gets the load configuration file command.
    /// </summary>
    ICommand LoadConfigurationFileCommand { get; }
}
