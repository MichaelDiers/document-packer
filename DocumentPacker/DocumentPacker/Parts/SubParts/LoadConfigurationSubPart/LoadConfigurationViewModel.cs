namespace DocumentPacker.Parts.SubParts.LoadConfigurationSubPart;

using System.ComponentModel;
using System.IO;
using System.Security;
using System.Windows.Input;
using DocumentPacker.Commands;
using DocumentPacker.Extensions;
using DocumentPacker.Models;
using DocumentPacker.Services;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;
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
    ///     The configuration file filter.
    /// </summary>
    private string configurationFileFilter;

    /// <summary>
    ///     The command to load the configuration file.
    /// </summary>
    private TranslatableButton<ICommand> loadConfigurationCommand;

    /// <summary>
    ///     The password to decrypt the document packer configuration file.
    /// </summary>
    private TranslatableAndValidable<SecureString> password = new(
        null,
        pwd => pwd.Value is not null && pwd.Value.Length > 0
            ? null
            : nameof(LoadConfigurationTranslation.PasswordIsRequired),
        false,
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
    /// <param name="loadPrivateConfiguration">
    ///     If <c>true</c> load a private configuration; load a public configuration
    ///     otherwise.
    /// </param>
    public LoadConfigurationViewModel(
        ICommandFactory commandFactory,
        IDocumentPackerConfigurationFileService configurationFileService,
        bool loadPrivateConfiguration
    )
    {
        this.configurationFileService = configurationFileService;

        this.configurationFile = new TranslatableAndValidable<string>(
            null,
            data => File.Exists(data.Value) ? null : nameof(LoadConfigurationTranslation.ConfigurationFileDoesNotExist),
            false,
            LoadConfigurationTranslation.ResourceManager,
            loadPrivateConfiguration
                ? nameof(LoadConfigurationTranslation.ConfigurationFileLabelPrivate)
                : nameof(LoadConfigurationTranslation.ConfigurationFileLabelPublic),
            nameof(LoadConfigurationTranslation.ConfigurationFileToolTip),
            loadPrivateConfiguration
                ? nameof(LoadConfigurationTranslation.ConfigurationFileWatermarkPrivate)
                : nameof(LoadConfigurationTranslation.ConfigurationFileWatermarkPublic));
        this.configurationFile.PropertyChanged += this.InvalidateConfiguration;

        this.loadConfigurationCommand = new TranslatableButton<ICommand>(
            commandFactory.CreateAsyncCommand<object?, ConfigurationModel?>(
                _ => this.LoadConfigurationCommandCanExecute(),
                null,
                (_, cancellationToken) => this.LoadConfigurationCommandExecute(cancellationToken),
                task =>
                {
                    if (task.Result is not null)
                    {
                        this.ConfigurationLoaded?.Invoke(
                            this,
                            new LoadConfigurationEventArgs(task.Result));
                    }
                }),
            "material_symbol_refresh.png".ToPackImage(),
            LoadConfigurationTranslation.ResourceManager,
            nameof(LoadConfigurationTranslation.LoadConfigurationCommandLabel),
            nameof(LoadConfigurationTranslation.LoadConfigurationCommandToolTip));

        this.selectConfigurationFileCommand = new SelectFileCommand(
            commandFactory,
            (_, path) => this.ConfigurationFile.Value = path,
            loadPrivateConfiguration
                ? "Document Packer Configuration (*.private.dpc)|*.private.dpc"
                : "Document Packer Configuration (*.public.dpc)|*.public.dpc");
        this.configurationFileFilter = loadPrivateConfiguration ? ".private.dpc" : ".public.dpc";
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
    ///     Gets or sets the configuration file filter.
    /// </summary>
    public string ConfigurationFileFilter
    {
        get => this.configurationFileFilter;
        set =>
            this.SetField(
                ref this.configurationFileFilter,
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
    public TranslatableAndValidable<SecureString> Password
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

    private bool LoadConfigurationCommandCanExecute()
    {
        this.ConfigurationFile.Validate();
        this.Password.Validate();

        return !this.ConfigurationFile.HasError && !this.Password.HasError;
    }

    /// <summary>
    ///     The execute function of the <see cref="LoadConfigurationCommand" />.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>The loaded <see cref="ConfigurationModel" />.</returns>
    private async Task<ConfigurationModel?> LoadConfigurationCommandExecute(CancellationToken cancellationToken)
    {
        if (!this.LoadConfigurationCommandCanExecute())
        {
            return null;
        }

        var configuration = await this.configurationFileService.FromFileAsync(
            new FileInfo(this.ConfigurationFile.Value!),
            this.password.Value!,
            cancellationToken);

        return configuration;
    }
}
