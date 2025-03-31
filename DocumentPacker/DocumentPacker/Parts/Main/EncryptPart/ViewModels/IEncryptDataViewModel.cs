namespace DocumentPacker.Parts.Main.EncryptPart.ViewModels;

using System.Collections.ObjectModel;
using Libs.Wpf.Localization;

/// <summary>
///     Describes the data that gets encrypted.
/// </summary>
public interface IEncryptDataViewModel
{
    /// <summary>
    ///     Gets or sets the description.
    /// </summary>
    TranslatableAndValidable<string>? Description { get; set; }

    /// <summary>
    ///     Gets or sets the items.
    /// </summary>
    ObservableCollection<IEncryptDataItemViewModel>? Items { get; set; }

    /// <summary>
    ///     Gets or sets the rsa public key.
    /// </summary>
    string RsaPublicKey { get; set; }

    /// <summary>
    ///     Validates the view model.
    /// </summary>
    /// <returns><c>True</c> if the validation succeeds; <c>false</c> otherwise.</returns>
    bool Validate();
}
