namespace DocumentPacker.Parts.Main.EncryptPart;

using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Main.EncryptPart.Service;
using Libs.Wpf.Commands;
using Microsoft.Win32;

/// <summary>
///     The view model of <see cref="EncryptView" />.
/// </summary>
/// <seealso cref="ApplicationBaseViewModel" />
internal class EncryptViewModel(IEncryptService encryptService, ICommandFactory commandFactory)
    : ApplicationBaseViewModel
{
    /// <summary>
    ///     The document packer output file.
    /// </summary>
    private string documentPackerOutputFile = $"{nameof(DocumentPacker)}.dp";

    /// <summary>
    ///     The document packer output folder.
    /// </summary>
    private string documentPackerOutputFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    /// <summary>
    ///     The encrypt items.
    /// </summary>
    private ObservableCollection<EncryptItemViewModel> encryptItems = new(
        [new EncryptItemViewModel {SelectedEncryptItemType = EncryptItemType.File}]);

    /// <summary>
    ///     The rsa private key pem.
    /// </summary>
    private string rsaPrivateKeyPem = string.Empty;

    /// <summary>
    ///     The rsa public key pem.
    /// </summary>
    private string rsaPublicKeyPem = string.Empty;

    /// <summary>
    ///     Gets the add encrypt item command.
    /// </summary>
    public ICommand AddEncryptItemCommand =>
        commandFactory.CreateSyncCommand(
            _ => true,
            _ => this.EncryptItems.Add(new EncryptItemViewModel()));

    /// <summary>
    ///     Gets the attach-file command.
    /// </summary>
    public ICommand AttachFileCommand =>
        commandFactory.CreateSyncCommand(
            obj => obj is EncryptItemViewModel,
            obj =>
            {
                if (obj is not EncryptItemViewModel encryptItemViewModel)
                {
                    return;
                }

                var dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    encryptItemViewModel.Value = dialog.FileName;
                }
            });

    /// <summary>
    ///     Gets the delete-encrypt item command.
    /// </summary>
    public ICommand DeleteEncryptItemCommand =>
        commandFactory.CreateSyncCommand(
            _ => true,
            obj =>
            {
                if (obj is not EncryptItemViewModel encryptItem)
                {
                    throw new ArgumentException(
                        nameof(EncryptItemViewModel),
                        nameof(obj));
                }

                this.EncryptItems.Remove(encryptItem);
            });

    /// <summary>
    ///     Gets or sets the document packer output file.
    /// </summary>
    public string DocumentPackerOutputFile
    {
        get => this.documentPackerOutputFile;
        set
        {
            this.SetField(
                ref this.documentPackerOutputFile,
                value);

            if (string.IsNullOrWhiteSpace(value))
            {
                this.SetError(EncryptPartTranslation.FileNameIsMissing);
            }
            else
            {
                this.ResetErrors();
            }
        }
    }

    /// <summary>
    ///     Gets or sets the document packer output folder.
    /// </summary>
    public string DocumentPackerOutputFolder
    {
        get => this.documentPackerOutputFolder;
        set
        {
            this.SetField(
                ref this.documentPackerOutputFolder,
                value);
            if (!Directory.Exists(value))
            {
                this.SetError(EncryptPartTranslation.DirectoryDoesNotExist);
            }
            else
            {
                this.ResetErrors();
            }
        }
    }

    /// <summary>
    ///     Gets the encrypt command.
    /// </summary>
    public ICommand EncryptCommand =>
        commandFactory.CreateAsyncCommand<object, FileInfo>(
            _ => this.Validate(),
            null,
            async (_, cancellationToken) => await this.EncryptAsync(cancellationToken),
            task =>
            {
                // Todo
                MessageBox.Show(
                    "Created file " + task.Result,
                    "caption",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            });

    /// <summary>
    ///     Gets or sets the encrypt items.
    /// </summary>
    public ObservableCollection<EncryptItemViewModel> EncryptItems
    {
        get => this.encryptItems;
        set =>
            this.SetField(
                ref this.encryptItems,
                value);
    }

    /// <summary>
    ///     Gets the generateRsaKeys command.
    /// </summary>
    public ICommand GenerateRsaKeysCommand =>
        commandFactory.CreateAsyncCommand<object, (string privateKey, string publicKey)>(
            _ => true,
            null,
            async (_, cancellationToken) => await encryptService.GenerateRsaKeysAsync(cancellationToken),
            task =>
            {
                var (privateKey, publicKey) = task.Result;
                this.RsaPrivateKeyPem = privateKey;
                this.RsaPublicKeyPem = publicKey;
            });

    /// <summary>
    ///     Gets or sets the rsa private key pem.
    /// </summary>
    public string RsaPrivateKeyPem
    {
        get => this.rsaPrivateKeyPem;
        set =>
            this.SetField(
                ref this.rsaPrivateKeyPem,
                value);
    }

    /// <summary>
    ///     Gets or sets the rsa public key pem.
    /// </summary>
    public string RsaPublicKeyPem
    {
        get => this.rsaPublicKeyPem;
        set =>
            this.SetField(
                ref this.rsaPublicKeyPem,
                value);
    }

    /// <summary>
    ///     Gets the selectDocumentPackerOutputFolder command.
    /// </summary>
    public ICommand SelectDocumentPackerOutputFolderCommand =>
        commandFactory.CreateSyncCommand(
            _ => true,
            _ =>
            {
                var dialog = new OpenFolderDialog {FolderName = this.DocumentPackerOutputFolder};
                var dialogResult = dialog.ShowDialog();
                if (dialogResult == true)
                {
                    this.DocumentPackerOutputFolder = dialog.FolderName;
                }
            });

    /// <summary>
    ///     Encrypts the collected data.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
    private async Task<FileInfo> EncryptAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(
            3000,
            cancellationToken);
        return new FileInfo(
            Path.Combine(
                this.DocumentPackerOutputFolder,
                this.documentPackerOutputFile));
        /**
        var (_, publicKey) = await encryptService.GenerateRsaKeysAsync(cancellationToken);
        var documentPackerFile = new FileInfo("file.dp");

        await encryptService.EncryptAsync(
            new EncryptData(
                documentPackerFile,
                this.EncryptItems.Select(
                    item => new EncryptDataElement(
                        item.Description,
                        item.EncryptItemType,
                        item.Value,
                        item.IsRequired)),
                publicKey),
            cancellationToken);
        **/
    }

    /// <summary>
    ///     Validates all <see cref="EncryptItems" />.
    /// </summary>
    /// <returns><c>True</c> if an item exists and all items are valid.</returns>
    private bool Validate()
    {
        return this.EncryptItems.Any() && this.EncryptItems.All(item => !item.HasErrors);
    }
}
