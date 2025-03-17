﻿namespace DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;

using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DocumentPacker.Commands;
using DocumentPacker.Extensions;
using DocumentPacker.Models;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Main.CreateConfigurationPart.Translations;
using DocumentPacker.Parts.Main.CreateConfigurationPart.Views;
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
    /// <summary>
    ///     A pattern to match the rsa private key format.
    /// </summary>
    private const string PrivatePemPattern = "^-----BEGIN RSA PRIVATE KEY-----.+?-----END RSA PRIVATE KEY-----$";

    /// <summary>
    ///     A pattern to match the rsa public key format.
    /// </summary>
    private const string PublicPemPattern = "^-----BEGIN RSA PUBLIC KEY-----.+?-----END RSA PUBLIC KEY-----$";

    /// <summary>
    ///     A regex to match the rsa private key format.
    /// </summary>
    private static readonly Regex PrivatePemRegex = new(
        CreateConfigurationViewModel.PrivatePemPattern,
        RegexOptions.Singleline);

    /// <summary>
    ///     A regex to match the rsa public key format.
    /// </summary>
    private static readonly Regex PublicPemRegex = new(
        CreateConfigurationViewModel.PublicPemPattern,
        RegexOptions.Singleline);

    /// <summary>
    ///     A service for creating a document packer configuration file.
    /// </summary>
    private readonly IDocumentPackerConfigurationFileService documentPackerConfigurationFileService;

    /// <summary>
    ///     The configuration items.
    /// </summary>
    private ObservableCollection<CreateConfigurationItemViewModel> configurationItems = new();

    /// <summary>
    ///     The description of the configuration.
    /// </summary>
    private TranslatableAndValidable<string> description = new(
        string.Empty,
        data => string.IsNullOrWhiteSpace(data.Value)
            ? nameof(CreateConfigurationPartTranslation.DescriptionIsRequired)
            : null,
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.DescriptionLabel),
        nameof(CreateConfigurationPartTranslation.DescriptionToolTip),
        nameof(CreateConfigurationPartTranslation.DescriptionWatermark));

    /// <summary>
    ///     The output folder.
    /// </summary>
    private TranslatableAndValidable<string> outputFolder = new(
        null,
        data =>
        {
            if (string.IsNullOrWhiteSpace(data.Value))
            {
                return nameof(CreateConfigurationPartTranslation.OutputFolderIsRequired);
            }

            if (!Directory.Exists(data.Value))
            {
                return nameof(CreateConfigurationPartTranslation.OutputFolderDoesNotExist);
            }

            return null;
        },
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.OutputFolderLabel),
        nameof(CreateConfigurationPartTranslation.OutputFolderToolTip),
        nameof(CreateConfigurationPartTranslation.OutputFolderWatermark));

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
    private TranslatableAndValidable<string> privateOutputFile;

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
    private TranslatableAndValidable<string> publicOutputFile;

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
        data =>
        {
            if (string.IsNullOrWhiteSpace(data.Value))
            {
                return nameof(CreateConfigurationPartTranslation.RsaPrivateKeyIsRequired);
            }

            if (!CreateConfigurationViewModel.PrivatePemRegex.IsMatch(data.Value))
            {
                return nameof(CreateConfigurationPartTranslation.RsaPrivateKeyFormatError);
            }

            return null;
        },
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.RsaPrivateKeyLabel),
        nameof(CreateConfigurationPartTranslation.RsaPrivateKeyToolTip),
        nameof(CreateConfigurationPartTranslation.RsaPrivateKeyWatermark));

    /// <summary>
    ///     The public rsa key in pem format.
    /// </summary>
    private TranslatableAndValidable<string> rsaPublicKey = new(
        null,
        data =>
        {
            if (string.IsNullOrWhiteSpace(data.Value))
            {
                return nameof(CreateConfigurationPartTranslation.RsaPublicKeyIsRequired);
            }

            if (!CreateConfigurationViewModel.PublicPemRegex.IsMatch(data.Value))
            {
                return nameof(CreateConfigurationPartTranslation.RsaPublicKeyFormatError);
            }

            return null;
        },
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.RsaPublicKeyLabel),
        nameof(CreateConfigurationPartTranslation.RsaPublicKeyToolTip),
        nameof(CreateConfigurationPartTranslation.RsaPublicKeyWatermark));

    /// <summary>
    ///     The description of the view.
    /// </summary>
    private Translatable viewDescription = new(
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.ViewDescription));

    /// <summary>
    ///     The headline of the view.
    /// </summary>
    private Translatable viewHeadline = new(
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.ViewHeadline));

    /// <summary>
    ///     The view model of <see cref="CreateConfigurationView" />.
    /// </summary>
    /// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
    public CreateConfigurationViewModel(
        ICommandFactory commandFactory,
        IRsaService rsaService,
        IDocumentPackerConfigurationFileService documentPackerConfigurationFileService,
        ICommandSync commandSync
    )
    {
        this.documentPackerConfigurationFileService = documentPackerConfigurationFileService;
        this.ConfigurationItems.Add(new CreateConfigurationItemViewModel());

        this.AddConfigurationItemCommand = new TranslatableButton<ICommand>(
            commandFactory.CreateSyncCommand(
                _ => !commandSync.IsCommandActive,
                _ => this.ConfigurationItems.Add(new CreateConfigurationItemViewModel())),
            "material_symbol_add.png".ToBitmapImage(),
            CreateConfigurationPartTranslation.ResourceManager,
            null,
            nameof(CreateConfigurationPartTranslation.AddConfigurationItemToolTip));

        this.DeleteConfigurationItemCommand = new TranslatableButton<ICommand>(
            commandFactory.CreateSyncCommand<CreateConfigurationItemViewModel>(
                item => !commandSync.IsCommandActive && item is not null,
                item =>
                {
                    if (item is not null)
                    {
                        this.ConfigurationItems.Remove(item);
                    }
                }),
            "material_symbol_delete.png".ToBitmapImage(),
            CreateConfigurationPartTranslation.ResourceManager,
            null,
            nameof(CreateConfigurationPartTranslation.DeleteConfigurationItemCommandToolTip));

        this.GenerateRsaKeysCommand = new TranslatableButton<IAsyncCommand>(
            commandFactory.CreateAsyncCommand<object, (string privateKey, string publicKey)>(
                _ => !commandSync.IsCommandActive,
                null,
                (_, _) =>
                {
                    if (!commandSync.Enter())
                    {
                        return Task.FromResult((string.Empty, string.Empty));
                    }

                    var keys = rsaService.GenerateKeys();
                    return Task.FromResult((keys.privateKey, keys.publicKey));
                },
                task =>
                {
                    try
                    {
                        var (privateKey, publicKey) = task.Result;
                        if (string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
                        {
                            return;
                        }

                        this.RsaPrivateKey.Value = privateKey;
                        this.RsaPublicKey.Value = publicKey;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"{CreateConfigurationViewModel.GetTranslation(() => CreateConfigurationPartTranslation.GenerateRsaKeysCommandUnknownError)}{ex.Message}",
                            CreateConfigurationViewModel.GetTranslation(
                                () => CreateConfigurationPartTranslation.GenerateRsaKeysCommandCaption),
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    finally
                    {
                        commandSync.Exit();
                    }
                }),
            "material_symbol_key.png".ToBitmapImage(),
            CreateConfigurationPartTranslation.ResourceManager,
            null,
            nameof(CreateConfigurationPartTranslation.GenerateRsaKeysCommandToolTip));

        this.SaveCommand = new TranslatableCancellableButton(
            commandFactory.CreateAsyncCommand<PasswordBox, (bool succeeds, string? message)>(
                passwordBox => !commandSync.IsCommandActive && this.Password.Validate(passwordBox?.Password),
                passwordBox => this.Validate(passwordBox),
                (passwordBox, cancellationToken) => CommandExecutor.Execute(
                    () => this.Validate(passwordBox),
                    commandSync,
                    () => this.SaveCommandExecute(
                        passwordBox,
                        cancellationToken),
                    this.SaveCommand),
                CommandExecutor.PostExecute),
            "material_symbol_save.png".ToBitmapImage(),
            CreateConfigurationPartTranslation.ResourceManager,
            nameof(CreateConfigurationPartTranslation.SaveLabel),
            nameof(CreateConfigurationPartTranslation.SaveToolTip),
            nameof(CreateConfigurationPartTranslation.CancelLabel),
            null,
            "material_symbol_cancel.png".ToBitmapImage(),
            nameof(CreateConfigurationPartTranslation.SaveCancelInfoText));

        this.SelectOutputFolderCommand = new TranslatableButton<ICommand>(
            commandFactory.CreateOpenFolderDialogCommand(
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)),
                folder => this.OutputFolder.Value = folder),
            "material_symbol_folder.png".ToBitmapImage(),
            CreateConfigurationPartTranslation.ResourceManager,
            null,
            nameof(CreateConfigurationPartTranslation.OpenFileDialogToolTip));

        this.privateOutputFile = new TranslatableAndValidable<string>(
            null,
            data =>
            {
                if (string.IsNullOrWhiteSpace(data.Value))
                {
                    return nameof(CreateConfigurationPartTranslation.PrivateOutputFileIsRequired);
                }

                if (!this.OutputFolder.HasError &&
                    !this.PrivateOutputFileExtension.HasError &&
                    File.Exists(
                        Path.Join(
                            this.OutputFolder.Value,
                            $"{data.Value}{this.PrivateOutputFileExtension.Value}")))
                {
                    return nameof(CreateConfigurationPartTranslation.PrivateOutputFileDoesExist);
                }

                return null;
            },
            false,
            CreateConfigurationPartTranslation.ResourceManager,
            nameof(CreateConfigurationPartTranslation.PrivateOutputFileLabel),
            nameof(CreateConfigurationPartTranslation.PrivateOutputFileToolTip),
            nameof(CreateConfigurationPartTranslation.PrivateOutputFileWatermark));

        this.publicOutputFile = new TranslatableAndValidable<string>(
            null,
            data =>
            {
                if (string.IsNullOrWhiteSpace(data.Value))
                {
                    return nameof(CreateConfigurationPartTranslation.PublicOutputFileIsRequired);
                }

                if (!this.OutputFolder.HasError &&
                    !this.PublicOutputFileExtension.HasError &&
                    File.Exists(
                        Path.Join(
                            this.OutputFolder.Value,
                            $"{data.Value}{this.PublicOutputFileExtension.Value}")))
                {
                    return nameof(CreateConfigurationPartTranslation.PublicOutputFileDoesExist);
                }

                return null;
            },
            false,
            CreateConfigurationPartTranslation.ResourceManager,
            nameof(CreateConfigurationPartTranslation.PublicOutputFileLabel),
            nameof(CreateConfigurationPartTranslation.PublicOutputFileToolTip),
            nameof(CreateConfigurationPartTranslation.PublicOutputFileWatermark));
    }

    /// <summary>
    ///     Gets the command to add a new configuration item.
    /// </summary>
    public TranslatableButton<ICommand> AddConfigurationItemCommand { get; }

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
    ///     Gets the command to delete a configuration item.
    /// </summary>
    public TranslatableButton<ICommand> DeleteConfigurationItemCommand { get; }

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

    /// <summary>
    ///     Gets a command to generate RSA keys.
    /// </summary>
    public TranslatableButton<IAsyncCommand> GenerateRsaKeysCommand { get; }

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
    ///     Gets the save command.
    /// </summary>
    public TranslatableCancellableButton SaveCommand { get; }

    /// <summary>
    ///     Gets the command to select the output folder.
    /// </summary>
    public TranslatableButton<ICommand> SelectOutputFolderCommand { get; }

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
    public bool Validate(PasswordBox? passwordBox)
    {
        var isValid = true;
        foreach (var createConfigurationItemViewModel in this.ConfigurationItems)
        {
            if (!createConfigurationItemViewModel.Validate())
            {
                isValid = false;
                continue;
            }

            var items = this.ConfigurationItems.Where(
                    item => string.Equals(
                        item.Id.Value,
                        createConfigurationItemViewModel.Id.Value,
                        StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (items.Length > 1)
            {
                isValid = false;
                foreach (var item in items)
                {
                    item.Id.ErrorResourceKey = nameof(CreateConfigurationPartTranslation.IdDuplicate);
                }
            }
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
               !this.Description.HasError &&
               !this.OutputFolder.HasError &&
               !this.Password.HasError &&
               !this.PrivateOutputFile.HasError &&
               !this.PrivateOutputFileExtension.HasError &&
               !this.PublicOutputFile.HasError &&
               !this.PublicOutputFileExtension.HasError &&
               !this.RsaPrivateKey.HasError &&
               !this.RsaPublicKey.HasError;
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
        var configuration = new ConfigurationModel(
            this.ConfigurationItems.Select(
                item => new ConfigurationItemModel(
                    item.ConfigurationItemTypes.SelectedValue!.Value,
                    item.IsRequired.Value,
                    item.ItemDescription.Value!,
                    item.Id.Value!)),
            this.Description.Value!,
            this.RsaPrivateKey.Value!,
            this.RsaPublicKey.Value!);

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
                passwordBox!.Password,
                configuration,
                cancellationToken);
            return (true,
                CreateConfigurationViewModel.GetTranslation(
                    () => CreateConfigurationPartTranslation.SaveConfigurationSucceeds));
        }
        catch (TaskCanceledException)
        {
            return (false, null);
        }
        catch
        {
            return (false,
                CreateConfigurationViewModel.GetTranslation(
                    () => CreateConfigurationPartTranslation.SaveConfigurationFails));
        }
    }
}
