﻿namespace DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;

using DocumentPacker.Parts.Main.CreateConfigurationPart.Translations;
using Libs.Wpf.ViewModels;

/// <summary>
///     Describes the data of a document packer configuration item.
/// </summary>
/// <seealso cref="ViewModelBase" />
public class CreateConfigurationItemViewModel : ViewModelBase
{
    /// <summary>
    ///     The configuration item types.
    /// </summary>
    private TranslatableAndValidableComboBox<ConfigurationItemType> configurationItemTypes = new(
        Enum.GetValues<ConfigurationItemType>()
            .Select(
                itemType => new TranslatableAndValidable<ConfigurationItemType>(
                    itemType,
                    null,
                    false,
                    CreateConfigurationPartTranslation.ResourceManager,
                    $"{nameof(ConfigurationItemType)}{itemType}")),
        data => data is TranslatableAndValidableComboBox<ConfigurationItemType> comboBoxData &&
                comboBoxData.SelectedValue is not null
            ? null
            : nameof(CreateConfigurationPartTranslation.SelectedConfigurationItemTypeMissing),
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.ConfigurationItemTypeLabel),
        null,
        nameof(CreateConfigurationPartTranslation.ConfigurationItemTypeWatermark));

    /// <summary>
    ///     A value that specifies if the value is required.
    /// </summary>
    private TranslatableAndValidable<bool> isRequired = new(
        false,
        null,
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.IsRequiredLabel));

    /// <summary>
    ///     The item description.
    /// </summary>
    private TranslatableAndValidable<string> itemDescription = new(
        null,
        data => string.IsNullOrWhiteSpace(data.Value)
            ? nameof(CreateConfigurationPartTranslation.ItemDescriptionErrorIsRequired)
            : null,
        false,
        CreateConfigurationPartTranslation.ResourceManager,
        nameof(CreateConfigurationPartTranslation.ItemDescriptionLabel),
        nameof(CreateConfigurationPartTranslation.ItemDescriptionToolTip),
        nameof(CreateConfigurationPartTranslation.ItemDescriptionWatermark));

    /// <summary>
    ///     Gets or sets the configuration item types.
    /// </summary>
    public TranslatableAndValidableComboBox<ConfigurationItemType> ConfigurationItemTypes
    {
        get => this.configurationItemTypes;
        set =>
            this.SetField(
                ref this.configurationItemTypes,
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
    public bool IsValid()
    {
        this.ConfigurationItemTypes.Validate();
        this.IsRequired.Validate();
        this.ItemDescription.Validate();

        return string.IsNullOrWhiteSpace(this.ConfigurationItemTypes.ErrorResourceKey) &&
               string.IsNullOrWhiteSpace(this.IsRequired.ErrorResourceKey) &&
               string.IsNullOrWhiteSpace(this.ItemDescription.ErrorResourceKey);
    }
}
