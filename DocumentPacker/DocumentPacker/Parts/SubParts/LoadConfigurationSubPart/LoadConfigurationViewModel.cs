﻿namespace DocumentPacker.Parts.SubParts.LoadConfigurationSubPart;

using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DocumentPacker.Commands;
using DocumentPacker.Models;
using DocumentPacker.Services;
using Libs.Wpf.Commands;
using Libs.Wpf.ViewModels;

/// <summary>
///     Describes the data of the <see cref="LoadConfigurationViewModel" />.
/// </summary>
internal class LoadConfigurationViewModel : ViewModelBase, IDisposable
{
    /// <summary>
    ///     A service to load the document packer configuration file.
    /// </summary>
    private readonly IDocumentPackerConfigurationFileService configurationFileService;

    /// <summary>
    ///     The document packer configuration file.
    /// </summary>
    private TranslatableAndValidable<string> configurationFile;

    /// <summary>
    ///     The command to load the configuration file.
    /// </summary>
    private TranslatableButton<ICommand> loadConfigurationCommand;

    /// <summary>
    ///     The password to decrypt the document packer configuration file.
    /// </summary>
    private Translatable password = new(
        LoadConfigurationTranslation.ResourceManager,
        nameof(LoadConfigurationTranslation.PasswordLabel),
        nameof(LoadConfigurationTranslation.PasswordToolTip),
        nameof(LoadConfigurationTranslation.PasswordWatermark));

    /// <summary>
    ///     The command to select the configuration file.
    /// </summary>
    private TranslatableButton<ICommand> selectConfigurationFileCommand;

    /// <summary>
    ///     Initializes a new instance of the <see cref="LoadConfigurationViewModel" /> class.
    /// </summary>
    /// <param name="commandFactory">A factory for creating commands.</param>
    /// <param name="configurationFileService">A service to load the document packer configuration file.</param>
    public LoadConfigurationViewModel(
        ICommandFactory commandFactory,
        IDocumentPackerConfigurationFileService configurationFileService
    )
    {
        this.configurationFileService = configurationFileService;

        this.configurationFile = new TranslatableAndValidable<string>(
            null,
            data => File.Exists(data.Value) ? null : nameof(LoadConfigurationTranslation.ConfigurationFileDoesNotExist),
            false,
            LoadConfigurationTranslation.ResourceManager,
            nameof(LoadConfigurationTranslation.ConfigurationFileLabel),
            nameof(LoadConfigurationTranslation.ConfigurationFileToolTip),
            nameof(LoadConfigurationTranslation.ConfigurationFileWatermark));
        this.configurationFile.PropertyChanged += this.InvalidateConfiguration;

        this.loadConfigurationCommand = new TranslatableButton<ICommand>(
            commandFactory.CreateAsyncCommand<PasswordBox, ConfigurationModel?>(
                this.LoadConfigurationCommandCanExecute,
                null,
                this.LoadConfigurationCommandExecute,
                task =>
                {
                    if (task.Result is not null)
                    {
                        this.ConfigurationLoaded?.Invoke(
                            this,
                            new LoadConfigurationEventArgs(task.Result));
                    }
                }),
            new BitmapImage(
                new Uri(
                    "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_refresh.png",
                    UriKind.Absolute)),
            LoadConfigurationTranslation.ResourceManager,
            nameof(LoadConfigurationTranslation.LoadConfigurationCommandLabel),
            nameof(LoadConfigurationTranslation.LoadConfigurationCommandToolTip));

        this.selectConfigurationFileCommand = new SelectFileCommand<object>(
            commandFactory,
            (_, path) => this.ConfigurationFile.Value = path,
            "Document Packer Configuration (*.public.dpc)|*.public.dpc");
    }

    /// <summary>
    ///     Gets or sets the configuration file.
    /// </summary>
    public TranslatableAndValidable<string> ConfigurationFile
    {
        get => this.configurationFile;
        protected set =>
            this.SetField(
                ref this.configurationFile,
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
    ///     Occurs if the document packer configuration is invalidated.
    /// </summary>
    public event EventHandler<EventArgs>? ConfigurationInvalidated;

    /// <summary>
    ///     Occurs if the document packer configuration is loaded.
    /// </summary>
    public event EventHandler<LoadConfigurationEventArgs>? ConfigurationLoaded;

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        this.configurationFile.PropertyChanged -= this.InvalidateConfiguration;
    }

    /// <summary>
    ///     Raises the <see cref="ConfigurationInvalidated" /> event if configuration file changed.
    /// </summary>
    /// <param name="sender">The sender that raised the event.</param>
    /// <param name="e">The event args.</param>
    private void InvalidateConfiguration(object? sender, PropertyChangedEventArgs e)
    {
        if (string.Equals(
                e.PropertyName,
                nameof(this.configurationFile.Value),
                StringComparison.OrdinalIgnoreCase))
        {
            this.ConfigurationInvalidated?.Invoke(
                this,
                EventArgs.Empty);
        }
    }

    private bool LoadConfigurationCommandCanExecute(PasswordBox? passwordBox)
    {
        this.ConfigurationFile.Validate();

        this.Password.ErrorResourceKey = string.IsNullOrWhiteSpace(passwordBox?.Password)
            ? nameof(LoadConfigurationTranslation.PasswordIsRequired)
            : null;

        return string.IsNullOrWhiteSpace(this.ConfigurationFile.ErrorResourceKey) &&
               string.IsNullOrWhiteSpace(this.Password.ErrorResourceKey);
    }

    /// <summary>
    ///     The execute function of the <see cref="LoadConfigurationCommand" />.
    /// </summary>
    /// <param name="passwordBox">The command parameter as a <see cref="PasswordBox" />.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>The loaded <see cref="ConfigurationModel" />.</returns>
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
}
