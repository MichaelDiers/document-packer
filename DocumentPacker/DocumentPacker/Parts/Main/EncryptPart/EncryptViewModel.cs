namespace DocumentPacker.Parts.Main.EncryptPart;

using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Main.EncryptPart.Model;
using DocumentPacker.Parts.Main.EncryptPart.Service;
using DocumentPacker.Resources;
using Microsoft.Win32;

/// <summary>
///     The view model of <see cref="EncryptView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationViewModel" />
internal class EncryptViewModel(IEncryptService encryptService) : ApplicationViewModel
{
    /// <summary>
    ///     The description text.
    /// </summary>
    private string description = Translation.FeaturesPartEncryptDescription;

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
        [new EncryptItemViewModel {EncryptItemType = EncryptItemType.File}]);

    /// <summary>
    ///     The headline text.
    /// </summary>
    private string headline = Translation.FeaturesPartEncryptHeadline;

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
        new SyncCommand(
            _ => true,
            _ => this.EncryptItems.Add(new EncryptItemViewModel()));

    /// <summary>
    ///     Gets the attach-file command.
    /// </summary>
    public ICommand AttachFileCommand =>
        new SyncCommand(
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
    ///     Gets the collapse all command.
    /// </summary>
    public ICommand CollapseAllCommand =>
        new SyncCommand(
            _ => true,
            collapse =>
            {
                if (collapse is not bool value)
                {
                    return;
                }

                foreach (var encryptItemViewModel in this.EncryptItems)
                {
                    encryptItemViewModel.IsExpanded = !value;
                }
            });

    /// <summary>
    ///     Gets the delete-encrypt item command.
    /// </summary>
    public ICommand DeleteEncryptItemCommand =>
        new SyncCommand(
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
    ///     Gets or sets the description text.
    /// </summary>
    public string Description
    {
        get => this.description;
        set =>
            this.SetField(
                ref this.description,
                value);
    }

    /// <summary>
    ///     Gets or sets the document packer output file.
    /// </summary>
    public string DocumentPackerOutputFile
    {
        get => this.documentPackerOutputFile;
        set =>
            this.SetField(
                ref this.documentPackerOutputFile,
                value,
                [
                    () => (nameof(EncryptViewModel.DocumentPackerOutputFile),
                        !string.IsNullOrWhiteSpace(value) ? [] : [Translation.FileNameIsMissing])
                ]);
    }

    /// <summary>
    ///     Gets or sets the document packer output folder.
    /// </summary>
    public string DocumentPackerOutputFolder
    {
        get => this.documentPackerOutputFolder;
        set =>
            this.SetField(
                ref this.documentPackerOutputFolder,
                value,
                [
                    () => (nameof(EncryptViewModel.DocumentPackerOutputFolder),
                        Directory.Exists(value) ? [] : [Translation.DirectoryDoesNotExist])
                ]);
    }

    /// <summary>
    ///     Gets the encrypt command.
    /// </summary>
    public ICommand EncryptCommand =>
        new TaskCommand(
            _ => this.Validate(),
            (_, cancellationToken) => this.EncryptAsync(cancellationToken));

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
        new TaskCommand(
            _ => true,
            async (_, cancellationToken) =>
            {
                var (privateKey, publicKey) = await encryptService.GenerateRsaKeysAsync(cancellationToken);
                this.RsaPrivateKeyPem = privateKey;
                this.RsaPublicKeyPem = publicKey;
            });

    /// <summary>
    ///     Gets or sets the headline text.
    /// </summary>
    public string Headline
    {
        get => this.headline;
        set =>
            this.SetField(
                ref this.headline,
                value);
    }

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
        new SyncCommand(
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
    private async Task EncryptAsync(CancellationToken cancellationToken)
    {
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
