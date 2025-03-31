namespace DocumentPacker.Parts.Main.DecryptPart;

using System.ComponentModel;
using System.Windows.Input;
using DocumentPacker.EventHandling;
using DocumentPacker.Parts.SubParts.LoadConfigurationSubPart;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;

/// <summary>
///     The view model of <see cref="DecryptView" />.
/// </summary>
public interface IDecryptViewModel : INotifyPropertyChanged, IApplicationViewModel
{
    /// <summary>
    ///     Gets or sets the command to decrypt the data.
    /// </summary>
    TranslatableButton<IAsyncCommand> DecryptCommand { get; set; }

    /// <summary>
    ///     Gets or sets the encrypted file.
    /// </summary>
    TranslatableAndValidable<string> EncryptedFile { get; set; }

    /// <summary>
    ///     Gets or sets the load configuration view model.
    /// </summary>
    ILoadConfigurationViewModel LoadConfigurationViewModel { get; set; }

    /// <summary>
    ///     Gets or sets the output folder.
    /// </summary>
    TranslatableAndValidable<string> OutputFolder { get; set; }

    /// <summary>
    ///     Gets or sets the private rsa key.
    /// </summary>
    TranslatableAndValidable<string> PrivateRsaKey { get; set; }

    /// <summary>
    ///     Gets or sets the command to select the encrypted file.
    /// </summary>
    TranslatableButton<ICommand> SelectEncryptedFileCommand { get; set; }

    /// <summary>
    ///     Gets or sets the command to select the output folder.
    /// </summary>
    TranslatableButton<ICommand> SelectOutputFolderCommand { get; set; }

    /// <summary>
    ///     Gets or sets the description of the view.
    /// </summary>
    Translatable ViewDescription { get; set; }

    /// <summary>
    ///     Gets or sets the headline of the view.
    /// </summary>
    Translatable ViewHeadline { get; set; }
}
