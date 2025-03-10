namespace DocumentPacker.Parts.Main.EncryptPart;

using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DocumentPacker.Commands;
using DocumentPacker.Models;
using DocumentPacker.Mvvm;
using DocumentPacker.Services;
using Libs.Wpf.Commands;
using Libs.Wpf.ViewModels;

/// <summary>
///     The view model of <see cref="EncryptView" />.
/// </summary>
/// <seealso cref="ApplicationBaseViewModel" />
internal class EncryptViewModel : ApplicationBaseViewModel
{
    private readonly IDocumentPackerConfigurationFileService configurationFileService;

    /// <summary>
    ///     The configuration file.
    /// </summary>
    private TranslatableAndValidable<string> configurationFile = new(
        null,
        data => File.Exists(data.Value) ? null : nameof(EncryptPartTranslation.ConfigurationFileDoesNotExist),
        false,
        EncryptPartTranslation.ResourceManager,
        nameof(EncryptPartTranslation.ConfigurationFileLabel),
        nameof(EncryptPartTranslation.ConfigurationFileToolTip),
        nameof(EncryptPartTranslation.ConfigurationFileWatermark));

    /// <summary>
    ///     The encrypt data view model.
    /// </summary>
    private EncryptDataViewModel? encryptDataViewModel;

    /// <summary>
    ///     The command to load the configuration file.
    /// </summary>
    private TranslatableButton<ICommand> loadConfigurationCommand;

    /// <summary>
    ///     The output file.
    /// </summary>
    private TranslatableAndValidable<string> outputFile;

    /// <summary>
    ///     The output folder.
    /// </summary>
    private TranslatableAndValidable<string> outputFolder;

    /// <summary>
    ///     The password.
    /// </summary>
    private Translatable password = new(
        EncryptPartTranslation.ResourceManager,
        nameof(EncryptPartTranslation.PasswordLabel),
        nameof(EncryptPartTranslation.PasswordWatermark));

    /// <summary>
    ///     The save command.
    /// </summary>
    private TranslatableButton<ICommand> saveCommand;

    /// <summary>
    ///     The command to select the configuration file.
    /// </summary>
    private TranslatableButton<ICommand> selectConfigurationFileCommand;

    /// <summary>
    ///     The command to select the output folder.
    /// </summary>
    private TranslatableButton<ICommand> selectOutputFolderCommand;

    /// <summary>
    ///     The description of the view.
    /// </summary>
    private Translatable viewDescription = new(
        EncryptPartTranslation.ResourceManager,
        nameof(EncryptPartTranslation.Description));

    /// <summary>
    ///     The headline of the view.
    /// </summary>
    private Translatable viewHeadline = new(
        EncryptPartTranslation.ResourceManager,
        nameof(EncryptPartTranslation.Headline));

    /// <summary>
    ///     The view model of <see cref="EncryptView" />.
    /// </summary>
    /// <seealso cref="ApplicationBaseViewModel" />
    public EncryptViewModel(
        ICommandFactory commandFactory,
        IDocumentPackerConfigurationFileService configurationFileService,
        IEncryptService encryptService
    )
    {
        this.configurationFileService = configurationFileService;

        this.loadConfigurationCommand = new TranslatableButton<ICommand>(
            commandFactory.CreateAsyncCommand<PasswordBox, ConfigurationModel?>(
                this.LoadConfigurationCommandCanExecute,
                null,
                this.LoadConfigurationCommandExecute,
                task =>
                {
                    this.EncryptDataViewModel = task.Result is null
                        ? null
                        : new EncryptDataViewModel(
                            task.Result,
                            commandFactory);
                }),
            new BitmapImage(
                new Uri(
                    "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_refresh.png",
                    UriKind.Absolute)),
            EncryptPartTranslation.ResourceManager,
            nameof(EncryptPartTranslation.LoadConfigurationCommandLabel),
            nameof(EncryptPartTranslation.LoadConfigurationCommandToolTip));

        this.outputFile = new TranslatableAndValidable<string>(
            null,
            data =>
            {
                if (string.IsNullOrWhiteSpace(data.Value))
                {
                    return nameof(EncryptPartTranslation.OutputFileIsRequired);
                }

                if (!this.OutputFolder.HasError &&
                    !string.IsNullOrWhiteSpace(this.OutputFolder.Value) &&
                    Path.Exists(
                        Path.Join(
                            this.OutputFolder.Value,
                            this.OutputFile.Value)))
                {
                    return nameof(EncryptPartTranslation.OutputFileExists);
                }

                return null;
            },
            false,
            EncryptPartTranslation.ResourceManager,
            nameof(EncryptPartTranslation.OutputFileLabel),
            nameof(EncryptPartTranslation.OutputFileToolTip),
            nameof(EncryptPartTranslation.OutputFileWatermark));

        this.outputFolder = new TranslatableAndValidable<string>(
            null,
            data =>
            {
                if (string.IsNullOrWhiteSpace(data.Value))
                {
                    return nameof(EncryptPartTranslation.OutputFolderIsRequired);
                }

                if (!Directory.Exists(data.Value))
                {
                    return nameof(EncryptPartTranslation.OutputFolderDoesNotExist);
                }

                return null;
            },
            false,
            EncryptPartTranslation.ResourceManager,
            nameof(EncryptPartTranslation.OutputFolderLabel),
            nameof(EncryptPartTranslation.OutputFolderToolTip),
            nameof(EncryptPartTranslation.OutputFolderWatermark));

        this.saveCommand = new TranslatableButton<ICommand>(
            commandFactory.CreateAsyncCommand<object, string?>(
                _ => this.Validate(),
                null,
                async (_, cancellationToken) => await this.SaveCommandExecuteAsync(
                    encryptService,
                    cancellationToken),
                task =>
                {
                    if (task.Result is null)
                    {
                        return;
                    }

                    MessageBox.Show(task.Result);
                }),
            new BitmapImage(
                new Uri(
                    "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_save.png",
                    UriKind.Absolute)),
            EncryptPartTranslation.ResourceManager,
            nameof(EncryptPartTranslation.SaveCommandLabel),
            nameof(EncryptPartTranslation.SaveCommandToolTip));

        this.selectConfigurationFileCommand = new SelectFileCommand<object>(
            commandFactory,
            (_, path) => this.ConfigurationFile.Value = path,
            "Document Packer Configuration (*.public.dpc)|*.public.dpc");

        this.selectOutputFolderCommand = new SelectFolderCommand(
            commandFactory,
            path => this.OutputFolder.Value = path);
    }

    /// <summary>
    ///     Gets or sets the configuration file.
    /// </summary>
    public TranslatableAndValidable<string> ConfigurationFile
    {
        get => this.configurationFile;
        set =>
            this.SetField(
                ref this.configurationFile,
                value);
    }

    /// <summary>
    ///     Gets or sets the encrypt data view model.
    /// </summary>
    public EncryptDataViewModel? EncryptDataViewModel
    {
        get => this.encryptDataViewModel;
        set =>
            this.SetField(
                ref this.encryptDataViewModel,
                value);
    }

    /// <summary>
    ///     Gets or sets the command to load the configuration file.
    /// </summary>
    public TranslatableButton<ICommand> LoadConfigurationCommand
    {
        get => this.loadConfigurationCommand;
        set =>
            this.SetField(
                ref this.loadConfigurationCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the output file.
    /// </summary>
    public TranslatableAndValidable<string> OutputFile
    {
        get => this.outputFile;
        set =>
            this.SetField(
                ref this.outputFile,
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
    public Translatable Password
    {
        get => this.password;
        set =>
            this.SetField(
                ref this.password,
                value);
    }

    /// <summary>
    ///     Gets or sets the save command.
    /// </summary>
    public TranslatableButton<ICommand> SaveCommand
    {
        get => this.saveCommand;
        set =>
            this.SetField(
                ref this.saveCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the command to select the configuration file.
    /// </summary>
    public TranslatableButton<ICommand> SelectConfigurationFileCommand
    {
        get => this.selectConfigurationFileCommand;
        set =>
            this.SetField(
                ref this.selectConfigurationFileCommand,
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

    private bool LoadConfigurationCommandCanExecute(PasswordBox? passwordBox)
    {
        this.ConfigurationFile.Validate();

        this.Password.ErrorResourceKey = string.IsNullOrWhiteSpace(passwordBox?.Password)
            ? nameof(EncryptPartTranslation.PasswordIsRequired)
            : null;

        return string.IsNullOrWhiteSpace(this.ConfigurationFile.ErrorResourceKey) &&
               string.IsNullOrWhiteSpace(this.Password.ErrorResourceKey);
    }

    private async Task<ConfigurationModel?> LoadConfigurationCommandExecute(
        PasswordBox? passwordBox,
        CancellationToken cancellationToken
    )
    {
        if (!this.LoadConfigurationCommandCanExecute(passwordBox))
        {
            return null;
        }

        var configuration = await this.configurationFileService.FromFileAsync(
            new FileInfo(this.ConfigurationFile.Value!),
            passwordBox!.Password,
            cancellationToken);

        return configuration;
    }

    private async Task<string?> SaveCommandExecuteAsync(
        IEncryptService encryptService,
        CancellationToken cancellationToken
    )
    {
        if (!this.Validate() ||
            string.IsNullOrWhiteSpace(this.OutputFolder.Value) ||
            string.IsNullOrWhiteSpace(this.OutputFile.Value) ||
            this.EncryptDataViewModel?.Items?.Any() != true)
        {
            return null;
        }

        var model = new EncryptModel(
            this.OutputFolder.Value,
            this.OutputFile.Value,
            this.EncryptDataViewModel.Items.Select(
                item => new EncryptItemModel(
                    item.ConfigurationItemType,
                    item.IsRequired.Value,
                    item.Value.Value,
                    item.Id)));
        await encryptService.EncryptAsync(
            this.EncryptDataViewModel.RsaPublicKey,
            model,
            cancellationToken);
        return "done";
    }

    /// <summary>
    ///     Validates the view model.
    /// </summary>
    /// <returns><c>True</c> if the validation succeeds; <c>false</c> otherwise.</returns>
    private bool Validate()
    {
        this.OutputFolder.Validate();
        this.OutputFile.Validate();

        return string.IsNullOrWhiteSpace(this.OutputFolder.ErrorResourceKey) &&
               string.IsNullOrWhiteSpace(this.OutputFile.ErrorResourceKey) &&
               this.EncryptDataViewModel?.Validate() == true;
    }
}
