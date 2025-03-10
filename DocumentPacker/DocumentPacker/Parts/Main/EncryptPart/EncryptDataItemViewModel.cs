namespace DocumentPacker.Parts.Main.EncryptPart;

using System.IO;
using System.Windows.Input;
using DocumentPacker.Commands;
using DocumentPacker.Models;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
using Libs.Wpf.Commands;
using Libs.Wpf.ViewModels;

public class EncryptDataItemViewModel : ViewModelBase
{
    /// <summary>
    ///     The configuration item type.
    /// </summary>
    private ConfigurationItemType configurationItemType;

    /// <summary>
    ///     The description.
    /// </summary>
    private TranslatableAndValidable<string> description;

    /// <summary>
    ///     The id.
    /// </summary>
    private string id;

    /// <summary>
    ///     The value that specifies if the value is required.
    /// </summary>
    private TranslatableAndValidable<bool> isRequired;

    /// <summary>
    ///     The select file command.
    /// </summary>
    private TranslatableButton<ICommand> selectFileCommand;

    /// <summary>
    ///     The value.
    /// </summary>
    private TranslatableAndValidable<string> value;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EncryptDataItemViewModel" /> class.
    /// </summary>
    public EncryptDataItemViewModel(ConfigurationItemModel configurationItemModel, ICommandFactory commandFactory)
    {
        this.configurationItemType = configurationItemModel.ConfigurationItemType;

        this.description = new TranslatableAndValidable<string>(
            configurationItemModel.ItemDescription,
            null,
            false,
            EncryptPartTranslation.ResourceManager,
            nameof(EncryptPartTranslation.DescriptionLabel));

        this.id = configurationItemModel.Id;

        this.isRequired = new TranslatableAndValidable<bool>(
            configurationItemModel.IsRequired,
            null,
            false,
            EncryptPartTranslation.ResourceManager,
            nameof(EncryptPartTranslation.IsRequiredLabel));

        this.selectFileCommand = new SelectFileCommand<object>(
            commandFactory,
            (_, path) => this.Value.Value = path);

        this.value = new TranslatableAndValidable<string>(
            null,
            data =>
            {
                switch (this.configurationItemType)
                {
                    case ConfigurationItemType.File:
                        if (string.IsNullOrWhiteSpace(data.Value))
                        {
                            return this.IsRequired.Value ? nameof(EncryptPartTranslation.FileIsRequired) : null;
                        }

                        return !File.Exists(data.Value) ? nameof(EncryptPartTranslation.FileDoesNotExist) : null;
                    case ConfigurationItemType.Text:
                        return !string.IsNullOrWhiteSpace(data.Value) || !this.IsRequired.Value
                            ? null
                            : nameof(EncryptPartTranslation.TextIsRequired);
                    default:
                        throw new ArgumentException($"Unsupported value: {this.configurationItemType}");
                }
            },
            false,
            EncryptPartTranslation.ResourceManager,
            $"{nameof(ConfigurationItemModel)}{configurationItemModel.ConfigurationItemType}Label",
            $"{nameof(ConfigurationItemModel)}{configurationItemModel.ConfigurationItemType}ToolTip",
            $"{nameof(ConfigurationItemModel)}{configurationItemModel.ConfigurationItemType}Watermark");
    }

    /// <summary>
    ///     Gets or sets the configuration item type.
    /// </summary>
    public ConfigurationItemType ConfigurationItemType
    {
        get => this.configurationItemType;
        set =>
            this.SetField(
                ref this.configurationItemType,
                value);
    }

    /// <summary>
    ///     Gets or sets the description.
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
    ///     Gets or sets the id.
    /// </summary>
    public string Id
    {
        get => this.id;
        set =>
            this.SetField(
                ref this.id,
                value);
    }

    /// <summary>
    ///     Gets or sets the value that specifies if the value is required.
    /// </summary>
    public TranslatableAndValidable<bool> IsRequired
    {
        get => this.isRequired;
        set =>
            this.SetField(
                ref this.isRequired,
                value);
    }

    /// <summary>
    ///     Gets or sets the select file command.
    /// </summary>
    public TranslatableButton<ICommand> SelectFileCommand
    {
        get => this.selectFileCommand;
        set =>
            this.SetField(
                ref this.selectFileCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the value.
    /// </summary>
    public TranslatableAndValidable<string> Value
    {
        get => this.value;
        set =>
            this.SetField(
                ref this.value,
                value);
    }

    /// <summary>
    ///     Validates the view model.
    /// </summary>
    /// <returns><c>True</c> if the validation succeeds; <c>false</c> otherwise.</returns>
    public bool Validate()
    {
        this.Value.Validate();

        return string.IsNullOrWhiteSpace(this.Value.ErrorResourceKey);
    }
}
