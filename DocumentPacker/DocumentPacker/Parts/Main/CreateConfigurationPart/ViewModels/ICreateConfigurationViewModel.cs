namespace DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security;
using System.Windows.Input;
using DocumentPacker.EventHandling;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;

/// <summary>
///     The create configuration view model.
/// </summary>
/// <seealso cref="IApplicationViewModel" />
/// <seealso cref="INotifyPropertyChanged" />
public interface ICreateConfigurationViewModel : IApplicationViewModel, INotifyPropertyChanged
{
    /// <summary>
    ///     Gets the command to add a new configuration item.
    /// </summary>
    TranslatableButton<ICommand> AddConfigurationItemCommand { get; }

    /// <summary>
    ///     Gets the configuration items.
    /// </summary>
    ObservableCollection<CreateConfigurationItemViewModel> ConfigurationItems { get; }

    /// <summary>
    ///     Gets the command to delete a configuration item.
    /// </summary>
    TranslatableButton<ICommand> DeleteConfigurationItemCommand { get; }

    /// <summary>
    ///     Gets the description of the configuration.
    /// </summary>
    TranslatableAndValidable<string> Description { get; }

    /// <summary>
    ///     Gets a command to generate RSA keys.
    /// </summary>
    TranslatableButton<IAsyncCommand> GenerateRsaKeysCommand { get; }

    /// <summary>
    ///     Gets the output folder.
    /// </summary>
    TranslatableAndValidable<string> OutputFolder { get; }

    /// <summary>
    ///     Gets the password.
    /// </summary>
    TranslatableAndValidable<SecureString> Password { get; }

    /// <summary>
    ///     Gets the private configuration output file.
    /// </summary>
    TranslatableAndValidable<string> PrivateOutputFile { get; }

    /// <summary>
    ///     Gets the private output file extension.
    /// </summary>
    TranslatableAndValidable<string> PrivateOutputFileExtension { get; }

    /// <summary>
    ///     Gets the public configuration output file.
    /// </summary>
    TranslatableAndValidable<string> PublicOutputFile { get; }

    /// <summary>
    ///     Gets the public output file extension.
    /// </summary>
    TranslatableAndValidable<string> PublicOutputFileExtension { get; }

    /// <summary>
    ///     Gets the private rsa key in pem format.
    /// </summary>
    TranslatableAndValidable<string> RsaPrivateKey { get; }

    /// <summary>
    ///     Gets the public rsa key in pem format.
    /// </summary>
    TranslatableAndValidable<string> RsaPublicKey { get; }

    /// <summary>
    ///     Gets the save command.
    /// </summary>
    TranslatableCancellableButton SaveCommand { get; }

    /// <summary>
    ///     Gets the command to select the output folder.
    /// </summary>
    TranslatableButton<ICommand> SelectOutputFolderCommand { get; }

    /// <summary>
    ///     Gets the description of the view.
    /// </summary>
    Translatable ViewDescription { get; }

    /// <summary>
    ///     Gets the headline of the view.
    /// </summary>
    Translatable ViewHeadline { get; }

    /// <summary>
    ///     Executes the view model validation.
    /// </summary>
    /// <returns><c>True</c> if the validation succeeds; <c>false</c> otherwise.</returns>
    bool Validate();
}
