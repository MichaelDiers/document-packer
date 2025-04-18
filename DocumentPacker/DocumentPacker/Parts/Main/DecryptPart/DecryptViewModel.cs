﻿namespace DocumentPacker.Parts.Main.DecryptPart;

using System.IO;
using System.Windows.Input;
using DocumentPacker.Commands;
using DocumentPacker.Extensions;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.SubParts.LoadConfigurationSubPart;
using DocumentPacker.Services;
using Libs.Wpf.Commands;
using Libs.Wpf.Controls.CustomMessageBox;
using Libs.Wpf.Localization;

/// <summary>
///     The view model of <see cref="DecryptView" />.
/// </summary>
/// <seealso cref="ApplicationBaseViewModel" />
/// <seealso cref="IDecryptViewModel" />
internal class DecryptViewModel : ApplicationBaseViewModel, IDecryptViewModel
{
    /// <summary>
    ///     The command to decrypt the data.
    /// </summary>
    private TranslatableButton<IAsyncCommand> decryptCommand;

    /// <summary>
    ///     The encrypted file.
    /// </summary>
    private TranslatableAndValidable<string> encryptedFile = new(
        string.Empty,
        data =>
        {
            if (string.IsNullOrWhiteSpace(data.Value))
            {
                return nameof(DecryptPartTranslation.EncryptedFileIsRequired);
            }

            return !File.Exists(data.Value) ? nameof(DecryptPartTranslation.EncryptedFileDoesNotExist) : null;
        },
        false,
        DecryptPartTranslation.ResourceManager,
        nameof(DecryptPartTranslation.EncryptedFileLabel),
        nameof(DecryptPartTranslation.EncryptedFileToolTip),
        nameof(DecryptPartTranslation.EncryptedFileWatermark));

    /// <summary>
    ///     The load configuration view model.
    /// </summary>
    private ILoadConfigurationViewModel loadConfigurationViewModel;

    /// <summary>
    ///     The output folder.
    /// </summary>
    private TranslatableAndValidable<string> outputFolder = new(
        string.Empty,
        data =>
        {
            if (string.IsNullOrWhiteSpace(data.Value))
            {
                return nameof(DecryptPartTranslation.OutputFolderIsRequired);
            }

            return !Directory.Exists(data.Value) ? nameof(DecryptPartTranslation.OutputFolderDoesNotExist) : null;
        },
        false,
        DecryptPartTranslation.ResourceManager,
        nameof(DecryptPartTranslation.OutputFolderLabel),
        nameof(DecryptPartTranslation.OutputFolderToolTip),
        nameof(DecryptPartTranslation.OutputFolderWatermark));

    /// <summary>
    ///     The private rsa key.
    /// </summary>
    private TranslatableAndValidable<string> privateRsaKey = new(
        string.Empty,
        data => string.IsNullOrWhiteSpace(data.Value) ? nameof(DecryptPartTranslation.PrivateRsaKeyIsRequired) : null,
        false,
        DecryptPartTranslation.ResourceManager,
        nameof(DecryptPartTranslation.PrivateRsaKeyLabel),
        nameof(DecryptPartTranslation.PrivateRsaKeyToolTip),
        nameof(DecryptPartTranslation.PrivateRsaKeyWatermark));

    /// <summary>
    ///     The command to select the encrypted file.
    /// </summary>
    private TranslatableButton<ICommand> selectEncryptedFileCommand;

    /// <summary>
    ///     The command to select the output folder.
    /// </summary>
    private TranslatableButton<ICommand> selectOutputFolderCommand;

    /// <summary>
    ///     The description of the view.
    /// </summary>
    private Translatable viewDescription = new(
        DecryptPartTranslation.ResourceManager,
        nameof(DecryptPartTranslation.Description));

    /// <summary>
    ///     The headline of the view.
    /// </summary>
    private Translatable viewHeadline = new(
        DecryptPartTranslation.ResourceManager,
        nameof(DecryptPartTranslation.Headline));

    /// <summary>
    ///     Initializes a new instance of the <see cref="DecryptViewModel" /> class.
    /// </summary>
    public DecryptViewModel(
        ICommandFactory commandFactory,
        IDocumentPackerConfigurationFileService configurationFileService,
        IDecryptService decryptService,
        ICommandSync commandSync,
        IMessageBoxService messageBoxService
    )
    {
        this.decryptCommand = new TranslatableButton<IAsyncCommand>(
            commandFactory.CreateAsyncCommand<object?, MessageBoxData?>(
                commandSync,
                _ => true,
                async (_, cancellationToken) =>
                {
                    if (!this.Validate())
                    {
                        return null;
                    }

                    await decryptService.DecryptAsync(
                        this.PrivateRsaKey.Value!,
                        new FileInfo(this.EncryptedFile.Value!),
                        new DirectoryInfo(this.OutputFolder.Value!),
                        cancellationToken);
                    var message = DecryptPartTranslation.ResourceManager.GetString(
                                      nameof(DecryptPartTranslation.DecryptCommandSucceeds),
                                      TranslationSource.Instance.CurrentCulture) ??
                                  "{0}";
                    return new MessageBoxData(
                        string.Format(
                            message,
                            this.OutputFolder.Value),
                        string.Empty,
                        MessageBoxButtons.Ok,
                        MessageBoxButtons.Ok,
                        MessageBoxImage.Information);
                },
                async (ex, _) =>
                {
                    var message = DecryptPartTranslation.ResourceManager.GetString(
                                      nameof(DecryptPartTranslation.DecryptCommandFails),
                                      TranslationSource.Instance.CurrentCulture) ??
                                  "{0}";
                    messageBoxService.Show(
                        new MessageBoxData(
                            string.Format(
                                message,
                                ex.Message),
                            string.Empty,
                            MessageBoxButtons.Ok,
                            MessageBoxButtons.Ok,
                            MessageBoxImage.Information));
                    await Task.CompletedTask;
                },
                translatableCancelButton: new TranslatableCancelButton(
                    DecryptPartTranslation.ResourceManager,
                    nameof(DecryptPartTranslation.DecryptCommandCancelLabel),
                    infoTextResourceKey: nameof(DecryptPartTranslation.DecryptCommandCancelInfoText),
                    imageSource: "material_symbol_cancel.png".ToPackImage()),
                postCommandFunc: async messageBoxData =>
                {
                    if (messageBoxData is null)
                    {
                        return;
                    }

                    messageBoxService.Show(messageBoxData);
                    await Task.CompletedTask;
                }),
            "material_symbol_expand.png".ToPackImage(),
            DecryptPartTranslation.ResourceManager,
            nameof(DecryptPartTranslation.DecryptCommandLabel),
            nameof(DecryptPartTranslation.DecryptCommandToolTip));

        this.loadConfigurationViewModel = new LoadConfigurationViewModel(
            commandFactory,
            configurationFileService,
            true,
            commandSync,
            messageBoxService);
        this.loadConfigurationViewModel.ConfigurationInvalidated += this.OnConfigurationInvalidated;
        this.loadConfigurationViewModel.ConfigurationLoaded += this.OnConfigurationLoaded;

        this.selectEncryptedFileCommand = new SelectFileCommand(
            commandFactory,
            (_, path) => this.EncryptedFile.Value = path,
            "Document Packer File (*.dp)|*.dp");

        this.selectOutputFolderCommand = new SelectFolderCommand(
            commandFactory,
            path => this.OutputFolder.Value = path);
    }

    /// <summary>
    ///     Gets or sets the command to decrypt the data.
    /// </summary>
    public TranslatableButton<IAsyncCommand> DecryptCommand
    {
        get => this.decryptCommand;
        set =>
            this.SetField(
                ref this.decryptCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the encrypted file.
    /// </summary>
    public TranslatableAndValidable<string> EncryptedFile
    {
        get => this.encryptedFile;
        set =>
            this.SetField(
                ref this.encryptedFile,
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
    ///     Gets or sets the private rsa key.
    /// </summary>
    public TranslatableAndValidable<string> PrivateRsaKey
    {
        get => this.privateRsaKey;
        set =>
            this.SetField(
                ref this.privateRsaKey,
                value);
    }

    /// <summary>
    ///     Gets or sets the command to select the encrypted file.
    /// </summary>
    public TranslatableButton<ICommand> SelectEncryptedFileCommand
    {
        get => this.selectEncryptedFileCommand;
        set =>
            this.SetField(
                ref this.selectEncryptedFileCommand,
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
    ///     Handles the <see cref="SubParts.LoadConfigurationSubPart.LoadConfigurationViewModel.ConfigurationInvalidated" />
    ///     event.
    /// </summary>
    /// <param name="sender">The sender that raised the event.</param>
    /// <param name="e">The event args.</param>
    private void OnConfigurationInvalidated(object? sender, EventArgs e)
    {
        this.PrivateRsaKey.Value = string.Empty;
    }

    /// <summary>
    ///     Handles the <see cref="SubParts.LoadConfigurationSubPart.LoadConfigurationViewModel.ConfigurationLoaded" /> event.
    /// </summary>
    /// <param name="sender">The sender that raised the event.</param>
    /// <param name="e">The event args.</param>
    private void OnConfigurationLoaded(object? sender, LoadConfigurationEventArgs e)
    {
        this.PrivateRsaKey.Value = e.ConfigurationModel.RsaPrivateKey;
    }

    /// <summary>
    ///     Validate all input related fields.
    /// </summary>
    /// <returns><c>True</c> if the view model is valid; <c>false</c> otherwise.</returns>
    private bool Validate()
    {
        this.EncryptedFile.Validate();
        this.PrivateRsaKey.Validate();
        this.OutputFolder.Validate();

        return !this.EncryptedFile.HasError && !this.PrivateRsaKey.HasError && !this.OutputFolder.HasError;
    }
}
