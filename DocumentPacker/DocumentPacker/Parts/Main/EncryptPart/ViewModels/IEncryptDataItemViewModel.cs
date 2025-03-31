namespace DocumentPacker.Parts.Main.EncryptPart.ViewModels;

using System.Windows.Input;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
using Libs.Wpf.Localization;

/// <summary>
///     Describes a data item that gets encrypted.
/// </summary>
public interface IEncryptDataItemViewModel
{
    /// <summary>
    ///     Gets or sets the configuration item type.
    /// </summary>
    ConfigurationItemType ConfigurationItemType { get; set; }

    /// <summary>
    ///     Gets or sets the description.
    /// </summary>
    TranslatableAndValidable<string> Description { get; set; }

    /// <summary>
    ///     Gets or sets the id.
    /// </summary>
    TranslatableAndValidable<string> Id { get; }

    /// <summary>
    ///     Gets or sets the value that specifies if the value is required.
    /// </summary>
    TranslatableAndValidable<bool> IsRequired { get; set; }

    /// <summary>
    ///     Gets or sets the select file command.
    /// </summary>
    TranslatableButton<ICommand> SelectFileCommand { get; set; }

    /// <summary>
    ///     Gets or sets the value.
    /// </summary>
    TranslatableAndValidable<string> Value { get; set; }

    /// <summary>
    ///     Validates the view model.
    /// </summary>
    /// <returns><c>True</c> if the validation succeeds; <c>false</c> otherwise.</returns>
    bool Validate();
}
