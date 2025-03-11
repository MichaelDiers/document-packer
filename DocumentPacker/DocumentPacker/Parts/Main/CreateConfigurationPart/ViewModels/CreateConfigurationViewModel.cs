namespace DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;

using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DocumentPacker.Models;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Main.CreateConfigurationPart.Translations;
using DocumentPacker.Services;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;
using Libs.Wpf.ViewModels;

/// <summary>
///     The view model of <see cref="CreateConfigurationView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class CreateConfigurationViewModel : ApplicationBaseViewModel
{
    private readonly IDocumentPackerConfigurationFileService documentPackerConfigurationFileService;
    private readonly IRsaService rsaService;

    /// <summary>
    ///     The command to add a new configuration item.
    /// </summary>
    private TranslatableButton<ICommand> addConfigurationItemCommand;

    /// <summary>
    ///     The configuration items.
    /// </summary>
    private ObservableCollection<CreateConfigurationItemViewModel> configurationItems = new();

    /// <summary>
    ///     The command to delete a configuration item.
    /// </summary>
    private TranslatableButton<ICommand> deleteConfigurationItemCommand;

    /// <summary>
    ///     The description of the configuration.
    /// </summary>
    private TranslatableAndValidable<string> description = new(
        null,
        data => string.IsNullOrWhiteSpace(data.Value)
            ? nameof(CreateConfigurationPartTranslation.DescriptionErrorIsRequired)
            : null,
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.DescriptionLabel),
        nameof(CreateConfigurationPartTranslation.DescriptionToolTip),
        nameof(CreateConfigurationPartTranslation.DescriptionWatermark));

    /// <summary>
    ///     A command to generate RSA keys.
    /// </summary>
    private TranslatableButton<ICommand> generateRsaKeysCommand;

    /// <summary>
    ///     The output folder.
    /// </summary>
    private TranslatableAndValidable<string> outputFolder = new(
        null,
        data => string.IsNullOrWhiteSpace(data.Value) || !Directory.Exists(data.Value)
            ? nameof(CreateConfigurationPartTranslation.OutputFolderIsRequired)
            : null,
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.OutputFolderLabel),
        nameof(CreateConfigurationPartTranslation.OutputFolderTextBoxToolTip),
        nameof(CreateConfigurationPartTranslation.OutputFolderTextBoxWatermark));

    /// <summary>
    ///     The password.
    /// </summary>
    private TranslatableAndValidablePasswordBox password = new(
        null,
        pwd => string.IsNullOrWhiteSpace(pwd) ? nameof(CreateConfigurationPartTranslation.PasswordIsRequired) : null,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.PasswordLabel),
        nameof(CreateConfigurationPartTranslation.PasswordToolTip),
        nameof(CreateConfigurationPartTranslation.PasswordWatermark));

    /// <summary>
    ///     The private configuration output file.
    /// </summary>
    private TranslatableAndValidable<string> privateOutputFile = new(
        null,
        data => string.IsNullOrWhiteSpace(data.Value)
            ? nameof(CreateConfigurationPartTranslation.PrivateOutputFileIsRequired)
            : null,
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.PrivateOutputFileLabel),
        nameof(CreateConfigurationPartTranslation.PrivateOutputFileToolTip),
        nameof(CreateConfigurationPartTranslation.PrivateOutputFileToolTip));

    /// <summary>
    ///     The private output file extension.
    /// </summary>
    private TranslatableAndValidable<string> privateOutputFileExtension = new(
        ".private.dpc",
        null,
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        null,
        nameof(CreateConfigurationPartTranslation.PrivateOutputFileExtensionToolTip));

    /// <summary>
    ///     The public configuration output file.
    /// </summary>
    private TranslatableAndValidable<string> publicOutputFile = new(
        null,
        data => string.IsNullOrWhiteSpace(data.Value)
            ? nameof(CreateConfigurationPartTranslation.PublicOutputFileIsRequired)
            : null,
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.PublicOutputFileLabel),
        nameof(CreateConfigurationPartTranslation.PublicOutputFileToolTip),
        nameof(CreateConfigurationPartTranslation.PublicOutputFileToolTip));

    /// <summary>
    ///     The public output file extension.
    /// </summary>
    private TranslatableAndValidable<string> publicOutputFileExtension = new(
        ".public.dpc",
        null,
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        null,
        nameof(CreateConfigurationPartTranslation.PublicOutputFileExtensionToolTip));

    /// <summary>
    ///     The private rsa key in pem format.
    /// </summary>
    private TranslatableAndValidable<string> rsaPrivateKey = new(
        null,
        data => string.IsNullOrWhiteSpace(data.Value)
            ? nameof(CreateConfigurationPartTranslation.RsaPrivateKeyIsRequired)
            : null,
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.RsaPrivateKeyLabel),
        nameof(CreateConfigurationPartTranslation.RsaPrivateKeyTextBoxToolTip),
        nameof(CreateConfigurationPartTranslation.RsaPrivateKeyTextBoxWatermark));

    /// <summary>
    ///     The public rsa key in pem format.
    /// </summary>
    private TranslatableAndValidable<string> rsaPublicKey = new(
        null,
        data => string.IsNullOrWhiteSpace(data.Value)
            ? nameof(CreateConfigurationPartTranslation.RsaPublicKeyIsRequired)
            : null,
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.RsaPublicKeyLabel),
        nameof(CreateConfigurationPartTranslation.RsaPublicKeyTextBoxToolTip),
        nameof(CreateConfigurationPartTranslation.RsaPublicKeyTextBoxWatermark));

    /// <summary>
    ///     The save command.
    /// </summary>
    private TranslatableButton<ICancellableCommand> saveCommand;

    /// <summary>
    ///     The command to select the output folder.
    /// </summary>
    private TranslatableButton<ICommand> selectOutputFolderCommand;

    /// <summary>
    ///     The description of the view.
    /// </summary>
    private Translatable viewDescription = new(
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.Description));

    /// <summary>
    ///     The headline of the view.
    /// </summary>
    private Translatable viewHeadline = new(
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.Headline));

    /// <summary>
    ///     The view model of <see cref="CreateConfigurationView" />.
    /// </summary>
    /// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
    public CreateConfigurationViewModel(
        ICommandFactory commandFactory,
        IRsaService rsaService,
        IDocumentPackerConfigurationFileService documentPackerConfigurationFileService
    )
    {
        this.rsaService = rsaService;
        this.documentPackerConfigurationFileService = documentPackerConfigurationFileService;
        this.ConfigurationItems.Add(new CreateConfigurationItemViewModel());

        this.addConfigurationItemCommand = new TranslatableButton<ICommand>(
            commandFactory.CreateSyncCommand(
                _ => true,
                _ => this.ConfigurationItems.Add(new CreateConfigurationItemViewModel())),
            new BitmapImage(
                new Uri(
                    "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_add.png",
                    UriKind.Absolute)),
            CreateConfigurationPartTranslation.ResourceManager,
            null,
            nameof(CreateConfigurationPartTranslation.AddConfigurationItemToolTip));

        this.deleteConfigurationItemCommand = new TranslatableButton<ICommand>(
            commandFactory.CreateSyncCommand<CreateConfigurationItemViewModel>(
                item => item is not null,
                item =>
                {
                    if (item is not null)
                    {
                        this.ConfigurationItems.Remove(item);
                    }
                }),
            new BitmapImage(
                new Uri(
                    "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_delete.png",
                    UriKind.Absolute)),
            CreateConfigurationPartTranslation.ResourceManager,
            null,
            nameof(CreateConfigurationPartTranslation.DeleteDescriptionItemToolTip));

        this.generateRsaKeysCommand = new TranslatableButton<ICommand>(
            commandFactory.CreateAsyncCommand<object, (string privateKey, string publicKey)>(
                null,
                null,
                (_, _) => Task.FromResult(rsaService.GenerateKeys()),
                task =>
                {
                    try
                    {
                        var (privateKey, publicKey) = task.Result;
                        this.RsaPrivateKey.Value = privateKey;
                        this.RsaPublicKey.Value = publicKey;
                    }
                    catch
                    {
                        // Todo
                    }
                }),
            new BitmapImage(
                new Uri(
                    "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_key.png",
                    UriKind.Absolute)),
            CreateConfigurationPartTranslation.ResourceManager,
            null,
            nameof(CreateConfigurationPartTranslation.GenerateRsaKeysToolTip));

        this.saveCommand = new TranslatableButton<ICancellableCommand>(
            commandFactory.CreateAsyncCommand<PasswordBox, (bool succeeds, string? message)>(
                null,
                passwordBox => this.IsValid(passwordBox),
                this.SaveCommandExecute,
                this.SaveCommandPostExecute),
            new BitmapImage(
                new Uri(
                    "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_save.png",
                    UriKind.Absolute)),
            CreateConfigurationPartTranslation.ResourceManager,
            nameof(CreateConfigurationPartTranslation.SaveLabel),
            nameof(CreateConfigurationPartTranslation.SaveToolTip));

        this.selectOutputFolderCommand = new TranslatableButton<ICommand>(
            commandFactory.CreateOpenFolderDialogCommand(
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)),
                folder => this.OutputFolder.Value = folder),
            new BitmapImage(
                new Uri(
                    "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_folder.png",
                    UriKind.Absolute)),
            CreateConfigurationPartTranslation.ResourceManager,
            null,
            nameof(CreateConfigurationPartTranslation.OpenFileDialogToolTip));
    }

    /// <summary>
    ///     Gets or sets the command to add a new configuration item.
    /// </summary>
    public TranslatableButton<ICommand> AddConfigurationItemCommand
    {
        get => this.addConfigurationItemCommand;
        set =>
            this.SetField(
                ref this.addConfigurationItemCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the configuration items.
    /// </summary>
    public ObservableCollection<CreateConfigurationItemViewModel> ConfigurationItems
    {
        get => this.configurationItems;
        set =>
            this.SetField(
                ref this.configurationItems,
                value);
    }

    /// <summary>
    ///     Gets or sets the command to delete a configuration item.
    /// </summary>
    public TranslatableButton<ICommand> DeleteConfigurationItemCommand
    {
        get => this.deleteConfigurationItemCommand;
        set =>
            this.SetField(
                ref this.deleteConfigurationItemCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the description of the configuration.
    /// </summary>
    public TranslatableAndValidable<string> Description
    {
        get => this.description;
        set =>
            this.SetField(
                ref this.description,
                value);
    }

    /// <summary>Gets an error message indicating what is wrong with this object.</summary>
    /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
    public string Error { get; } = "";

    /// <summary>
    ///     Gets or sets a command to generate RSA keys.
    /// </summary>
    public TranslatableButton<ICommand> GenerateRsaKeysCommand
    {
        get => this.generateRsaKeysCommand;
        set =>
            this.SetField(
                ref this.generateRsaKeysCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the output folder.
    /// </summary>
    public TranslatableAndValidable<string> OutputFolder
    {
        get => this.outputFolder;
        set =>
            this.SetField(
                ref this.outputFolder,
                value);
    }

    /// <summary>
    ///     Gets or sets the password.
    /// </summary>
    public TranslatableAndValidablePasswordBox Password
    {
        get => this.password;
        set =>
            this.SetField(
                ref this.password,
                value);
    }

    /// <summary>
    ///     Gets or sets the private configuration output file.
    /// </summary>
    public TranslatableAndValidable<string> PrivateOutputFile
    {
        get => this.privateOutputFile;
        set =>
            this.SetField(
                ref this.privateOutputFile,
                value);
    }

    /// <summary>
    ///     Gets or sets the private output file extension.
    /// </summary>
    public TranslatableAndValidable<string> PrivateOutputFileExtension
    {
        get => this.privateOutputFileExtension;
        set =>
            this.SetField(
                ref this.privateOutputFileExtension,
                value);
    }

    /// <summary>
    ///     Gets or sets the public configuration output file.
    /// </summary>
    public TranslatableAndValidable<string> PublicOutputFile
    {
        get => this.publicOutputFile;
        set =>
            this.SetField(
                ref this.publicOutputFile,
                value);
    }

    /// <summary>
    ///     Gets or sets the public output file extension.
    /// </summary>
    public TranslatableAndValidable<string> PublicOutputFileExtension
    {
        get => this.publicOutputFileExtension;
        set =>
            this.SetField(
                ref this.publicOutputFileExtension,
                value);
    }

    /// <summary>
    ///     Gets or sets the private rsa key in pem format.
    /// </summary>
    public TranslatableAndValidable<string> RsaPrivateKey
    {
        get => this.rsaPrivateKey;
        set =>
            this.SetField(
                ref this.rsaPrivateKey,
                value);
    }

    /// <summary>
    ///     Gets or sets the public rsa key in pem format.
    /// </summary>
    public TranslatableAndValidable<string> RsaPublicKey
    {
        get => this.rsaPublicKey;
        set =>
            this.SetField(
                ref this.rsaPublicKey,
                value);
    }

    /// <summary>
    ///     Gets or sets the save command.
    /// </summary>
    public TranslatableButton<ICancellableCommand> SaveCommand
    {
        get => this.saveCommand;
        set =>
            this.SetField(
                ref this.saveCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the command to select the output folder.
    /// </summary>
    public TranslatableButton<ICommand> SelectOutputFolderCommand
    {
        get => this.selectOutputFolderCommand;
        set =>
            this.SetField(
                ref this.selectOutputFolderCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the description of the view.
    /// </summary>
    public Translatable ViewDescription
    {
        get => this.viewDescription;
        set =>
            this.SetField(
                ref this.viewDescription,
                value);
    }

    /// <summary>
    ///     Gets or sets the headline of the view.
    /// </summary>
    public Translatable ViewHeadline
    {
        get => this.viewHeadline;
        set =>
            this.SetField(
                ref this.viewHeadline,
                value);
    }

    /// <summary>
    ///     Executes the view model validation.
    /// </summary>
    /// <returns><c>True</c> if the validation succeeds; <c>false</c> otherwise.</returns>
    public bool IsValid(PasswordBox? passwordBox)
    {
        var isValid = true;
        foreach (var createConfigurationItemViewModel in this.ConfigurationItems)
        {
            isValid = isValid && createConfigurationItemViewModel.IsValid();
        }

        this.Description.Validate();
        this.OutputFolder.Validate();
        this.Password.Validate(passwordBox?.Password);
        this.PrivateOutputFile.Validate();
        this.PrivateOutputFileExtension.Validate();
        this.PublicOutputFile.Validate();
        this.PublicOutputFileExtension.Validate();
        this.RsaPrivateKey.Validate();
        this.RsaPublicKey.Validate();

        return isValid &&
               string.IsNullOrWhiteSpace(this.Description.ErrorResourceKey) &&
               string.IsNullOrWhiteSpace(this.OutputFolder.ErrorResourceKey) &&
               string.IsNullOrWhiteSpace(this.Password.ErrorResourceKey) &&
               string.IsNullOrWhiteSpace(this.PrivateOutputFile.ErrorResourceKey) &&
               string.IsNullOrWhiteSpace(this.PrivateOutputFileExtension.ErrorResourceKey) &&
               string.IsNullOrWhiteSpace(this.PublicOutputFile.ErrorResourceKey) &&
               string.IsNullOrWhiteSpace(this.PublicOutputFileExtension.ErrorResourceKey) &&
               string.IsNullOrWhiteSpace(this.RsaPrivateKey.ErrorResourceKey) &&
               string.IsNullOrWhiteSpace(this.RsaPublicKey.ErrorResourceKey);
    }

    /// <summary>
    ///     Resets the <see cref="CultureInfo" /> and gets the requested <paramref name="getTranslation" />.
    /// </summary>
    /// <param name="getTranslation">A function that gets the requested translation.</param>
    /// <returns>The requested translation.</returns>
    internal static string GetTranslation(Func<string> getTranslation)
    {
        var cultureInfo = CreateConfigurationPartTranslation.Culture;
        CreateConfigurationPartTranslation.Culture = TranslationSource.Instance.CurrentCulture;
        var translation = getTranslation();
        CreateConfigurationPartTranslation.Culture = cultureInfo;
        return translation;
    }

    private async Task<(bool succeeds, string? message)> SaveCommandExecute(
        PasswordBox? passwordBox,
        CancellationToken cancellationToken
    )
    {
        if (!this.IsValid(passwordBox) || passwordBox is null)
        {
            return (false, null);
        }

        var configuration = new ConfigurationModel(
            this.ConfigurationItems.Select(
                item => new ConfigurationItemModel(
                    item.ConfigurationItemTypes.SelectedValue.Value,
                    item.IsRequired.Value,
                    item.ItemDescription.Value,
                    Guid.NewGuid().ToString())),
            this.Description.Value,
            this.RsaPrivateKey.Value,
            this.RsaPublicKey.Value);

        var privateConfigurationFile = new FileInfo(
            Path.Join(
                this.OutputFolder.Value,
                $"{this.PrivateOutputFile.Value}{this.PrivateOutputFileExtension.Value}"));
        var publicConfigurationFile = new FileInfo(
            Path.Join(
                this.OutputFolder.Value,
                $"{this.PublicOutputFile.Value}{this.PublicOutputFileExtension.Value}"));

        try
        {
            await this.documentPackerConfigurationFileService.ToFileAsync(
                privateConfigurationFile,
                publicConfigurationFile,
                passwordBox.Password,
                configuration,
                cancellationToken);
            return (true,
                CreateConfigurationViewModel.GetTranslation(
                    () => CreateConfigurationPartTranslation.SaveConfigurationSucceeds));
        }
        catch
        {
            return (false,
                CreateConfigurationViewModel.GetTranslation(
                    () => CreateConfigurationPartTranslation.SaveConfigurationFails));
        }
    }

    private void SaveCommandPostExecute(Task<(bool succeeds, string? message)> task)
    {
        var (succeeds, message) = task.Result;
        if (!succeeds && message is null)
        {
            return;
        }

        MessageBox.Show(
            message,
            string.Empty,
            MessageBoxButton.OK,
            !succeeds ? MessageBoxImage.Error : MessageBoxImage.Information,
            MessageBoxResult.OK);
    }
}
