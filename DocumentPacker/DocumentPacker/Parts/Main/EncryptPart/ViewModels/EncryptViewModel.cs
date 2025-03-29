namespace DocumentPacker.Parts.Main.EncryptPart.ViewModels;

using System.IO;
using System.Windows.Input;
using DocumentPacker.Commands;
using DocumentPacker.Extensions;
using DocumentPacker.Models;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
using DocumentPacker.Parts.Main.EncryptPart.Translations;
using DocumentPacker.Parts.Main.EncryptPart.Views;
using DocumentPacker.Parts.SubParts.LoadConfigurationSubPart;
using DocumentPacker.Services;
using Libs.Wpf.Commands;
using Libs.Wpf.Controls.CustomMessageBox;
using Libs.Wpf.Localization;

/// <summary>
///     The view model of <see cref="EncryptView" />.
/// </summary>
/// <seealso cref="ApplicationBaseViewModel" />
internal class EncryptViewModel : ApplicationBaseViewModel, IEncryptViewModel
{
    private readonly ICommandFactory commandFactory;

    /// <summary>
    ///     The encrypt data view model.
    /// </summary>
    private IEncryptDataViewModel? encryptDataViewModel;

    /// <summary>
    ///     The load configuration view model.
    /// </summary>
    private ILoadConfigurationViewModel loadConfigurationViewModel;

    /// <summary>
    ///     The output file.
    /// </summary>
    private TranslatableAndValidable<string> outputFile;

    /// <summary>
    ///     The output folder.
    /// </summary>
    private TranslatableAndValidable<string> outputFolder;

    /// <summary>
    ///     The save command.
    /// </summary>
    private TranslatableButton<IAsyncCommand> saveCommand;

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
        IEncryptService encryptService,
        ICommandSync commandSync,
        IMessageBoxService messageBoxService
    )
    {
        this.commandFactory = commandFactory;

        this.loadConfigurationViewModel = new LoadConfigurationViewModel(
            commandFactory,
            configurationFileService,
            false,
            commandSync,
            messageBoxService);
        this.loadConfigurationViewModel.ConfigurationLoaded += this.OnConfigurationLoaded;
        this.loadConfigurationViewModel.ConfigurationInvalidated += this.OnConfigurationInvalidated;

        this.outputFile = new TranslatableAndValidable<string>(
            null,
            data =>
            {
                if (string.IsNullOrWhiteSpace(data.Value))
                {
                    return nameof(EncryptPartTranslation.OutputFileIsRequired);
                }

                if (!this.OutputFolder.HasError &&
                    !this.OutputFileExtension.HasError &&
                    !string.IsNullOrWhiteSpace(this.OutputFolder.Value) &&
                    Path.Exists(
                        Path.Join(
                            this.OutputFolder.Value,
                            $"{this.OutputFile.Value}{this.OutputFileExtension.Value}")))
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

                return !Directory.Exists(data.Value) ? nameof(EncryptPartTranslation.OutputFolderDoesNotExist) : null;
            },
            false,
            EncryptPartTranslation.ResourceManager,
            nameof(EncryptPartTranslation.OutputFolderLabel),
            nameof(EncryptPartTranslation.OutputFolderToolTip),
            nameof(EncryptPartTranslation.OutputFolderWatermark));

        this.saveCommand = new TranslatableButton<IAsyncCommand>(
            commandFactory.CreateAsyncCommand(
                commandSync,
                () => true,
                async cancellationToken =>
                {
                    if (!this.Validate())
                    {
                        return;
                    }

                    var result = await this.SaveCommandExecuteAsync(
                        encryptService,
                        cancellationToken);
                    messageBoxService.Show(
                        result.message ?? string.Empty,
                        string.Empty,
                        MessageBoxButtons.Ok,
                        MessageBoxButtons.Ok,
                        result.success ? MessageBoxImage.Information : MessageBoxImage.Error);
                },
                async (ex, _) =>
                {
                    messageBoxService.Show(
                        ex.Message,
                        string.Empty,
                        MessageBoxButtons.Ok,
                        MessageBoxButtons.Ok,
                        MessageBoxImage.Error);
                    await Task.CompletedTask;
                },
                translatableCancelButton: new TranslatableCancelButton(
                    EncryptPartTranslation.ResourceManager,
                    nameof(EncryptPartTranslation.SaveCommandCancelLabel),
                    infoTextResourceKey: nameof(EncryptPartTranslation.SaveCommandCancelInfoText),
                    imageSource: "material_symbol_cancel.png".ToPackImage())),
            "material_symbol_save.png".ToPackImage(),
            EncryptPartTranslation.ResourceManager,
            nameof(EncryptPartTranslation.SaveCommandLabel),
            nameof(EncryptPartTranslation.SaveCommandToolTip));

        this.selectOutputFolderCommand = new SelectFolderCommand(
            commandFactory,
            path => this.OutputFolder.Value = path);
    }

    /// <summary>
    ///     Gets or sets the encrypt data view model.
    /// </summary>
    public IEncryptDataViewModel? EncryptDataViewModel
    {
        get => this.encryptDataViewModel;
        set =>
            this.SetField(
                ref this.encryptDataViewModel,
                value);
    }

    /// <summary>
    ///     Gets or sets the load configuration view model.
    /// </summary>
    public ILoadConfigurationViewModel LoadConfigurationViewModel
    {
        get => this.loadConfigurationViewModel;
        set =>
            this.SetField(
                ref this.loadConfigurationViewModel,
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
    ///     Gets or sets the output file.
    /// </summary>
    public TranslatableAndValidable<string> OutputFileExtension { get; } = new(
        ".dp",
        null,
        false,
        EncryptPartTranslation.ResourceManager,
        toolTipResourceKey: nameof(EncryptPartTranslation.OutputFileExtensionToolTip));

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
    ///     Gets or sets the save command.
    /// </summary>
    public TranslatableButton<IAsyncCommand> SaveCommand
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

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public override void Dispose()
    {
        this.LoadConfigurationViewModel.ConfigurationLoaded -= this.OnConfigurationLoaded;
        this.LoadConfigurationViewModel.ConfigurationInvalidated -= this.OnConfigurationInvalidated;

        base.Dispose();
    }

    private void OnConfigurationInvalidated(object? sender, EventArgs e)
    {
        this.EncryptDataViewModel = null;
    }

    private void OnConfigurationLoaded(object? sender, LoadConfigurationEventArgs e)
    {
        this.EncryptDataViewModel = new EncryptDataViewModel(
            e.ConfigurationModel,
            this.commandFactory);
    }

    private async Task<(bool success, string? message)> SaveCommandExecuteAsync(
        IEncryptService encryptService,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var model = new EncryptModel(
                this.OutputFolder.Value!,
                $"{this.OutputFile.Value}{this.OutputFileExtension.Value}",
                this.EncryptDataViewModel!.Items!.Select(
                    item =>
                    {
                        var id = item.Id.Value;
                        var value = item.Value.Value;

                        switch (item.ConfigurationItemType)
                        {
                            case ConfigurationItemType.File:
                                id = $"{id}{Path.GetExtension(value)}";
                                break;
                            case ConfigurationItemType.Text:
                                id = $"{id}.txt";
                                value = $"{item.Description.Value}{Environment.NewLine}{Environment.NewLine}{value}";
                                break;
                            default:
                                throw new ArgumentException($"Unsupported value: {item.ConfigurationItemType}");
                        }

                        return new EncryptItemModel(
                            item.ConfigurationItemType,
                            item.IsRequired.Value,
                            value,
                            id);
                    }));

            await encryptService.EncryptAsync(
                this.EncryptDataViewModel.RsaPublicKey,
                model,
                cancellationToken);
            var message = EncryptPartTranslation.ResourceManager.GetString(
                              nameof(EncryptPartTranslation.EncryptSucceeds),
                              TranslationSource.Instance.CurrentCulture) ??
                          "{0}";
            return (true, string.Format(
                message,
                model.OutputFile));
        }
        catch (Exception ex)
        {
            var message = EncryptPartTranslation.ResourceManager.GetString(
                              nameof(EncryptPartTranslation.EncryptFails),
                              TranslationSource.Instance.CurrentCulture) ??
                          "{0}";
            return (false, string.Format(
                message,
                ex.Message));
        }
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
