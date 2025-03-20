namespace DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;

using System.IO;
using DocumentPacker.Parts.Main.CreateConfigurationPart.Translations;
using Libs.Wpf.Localization;
using Libs.Wpf.ViewModels;

/// <summary>
///     Describes the data of a document packer configuration item.
/// </summary>
/// <seealso cref="ViewModelBase" />
public class CreateConfigurationItemViewModel : ViewModelBase
{
    /// <summary>
    ///     The id.
    /// </summary>
    private TranslatableAndValidable<string> id = new(
        string.Empty,
        data =>
        {
            if (string.IsNullOrWhiteSpace(data.Value))
            {
                return nameof(CreateConfigurationPartTranslation.IdIsRequired);
            }

            var invalidCharacters = Path.GetInvalidFileNameChars();
            return data.Value.Any(c => invalidCharacters.Contains(c))
                ? nameof(CreateConfigurationPartTranslation.IdConatainsInvalidCharacters)
                : null;
        },
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.IdLabel),
        nameof(CreateConfigurationPartTranslation.IdToolTip),
        nameof(CreateConfigurationPartTranslation.IdWatermark));

    /// <summary>
    ///     A value that specifies if the value is required.
    /// </summary>
    private TranslatableAndValidable<bool> isRequired = new(
        false,
        null,
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.IsRequiredLabel),
        nameof(CreateConfigurationPartTranslation.IsRequiredToolTip));

    /// <summary>
    ///     The item description.
    /// </summary>
    private TranslatableAndValidable<string> itemDescription = new(
        null,
        data => string.IsNullOrWhiteSpace(data.Value)
            ? nameof(CreateConfigurationPartTranslation.ItemDescriptionIsRequired)
            : null,
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.ItemDescriptionLabel),
        nameof(CreateConfigurationPartTranslation.ItemDescriptionToolTip),
        nameof(CreateConfigurationPartTranslation.ItemDescriptionWatermark));

    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateConfigurationItemViewModel" /> class.
    /// </summary>
    public CreateConfigurationItemViewModel()
    {
        this.ConfigurationItemTypes = new TranslatableAndValidableComboBox<ConfigurationItemType>(
            Enum.GetValues<ConfigurationItemType>()
                .Select(
                    itemType => new TranslatableAndValidable<ConfigurationItemType>(
                        itemType,
                        null,
                        false,
                        CreateConfigurationPartTranslation.ResourceManager,
                        $"{nameof(ConfigurationItemType)}{itemType}")),
            data => data is TranslatableAndValidableComboBox<ConfigurationItemType> {SelectedValue: not null}
                ? null
                : nameof(CreateConfigurationPartTranslation.SelectedConfigurationItemTypeMissing),
            false,
            CreateConfigurationPartTranslation.ResourceManager,
            nameof(CreateConfigurationPartTranslation.ConfigurationItemTypeLabel),
            nameof(CreateConfigurationPartTranslation.ConfigurationItemTypeToolTip),
            nameof(CreateConfigurationPartTranslation.ConfigurationItemTypeWatermark));
    }

    /// <summary>
    ///     Gets the configuration item types.
    /// </summary>
    public TranslatableAndValidableComboBox<ConfigurationItemType> ConfigurationItemTypes { get; }

    /// <summary>
    ///     Gets or sets the id.
    /// </summary>
    public TranslatableAndValidable<string> Id
    {
        get => this.id;
        set =>
            this.SetField(
                ref this.id,
                value);
    }

    /// <summary>
    ///     Gets or sets a value that specifies if the value is required.
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
    ///     Gets or sets the item description.
    /// </summary>
    public TranslatableAndValidable<string> ItemDescription
    {
        get => this.itemDescription;
        set =>
            this.SetField(
                ref this.itemDescription,
                value);
    }

    /// <summary>
    ///     Executes the view model validation.
    /// </summary>
    /// <returns><c>True</c> if the validation succeeds; <c>false</c> otherwise.</returns>
    public bool Validate()
    {
        this.ConfigurationItemTypes.Validate();
        this.IsRequired.Validate();
        this.ItemDescription.Validate();
        this.Id.Validate();

        return !this.ConfigurationItemTypes.HasError &&
               !this.IsRequired.HasError &&
               !this.ItemDescription.HasError &&
               !this.Id.HasError;
    }
}
