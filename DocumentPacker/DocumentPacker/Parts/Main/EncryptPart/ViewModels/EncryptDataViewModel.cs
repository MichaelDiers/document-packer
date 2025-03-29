namespace DocumentPacker.Parts.Main.EncryptPart.ViewModels;

using System.Collections.ObjectModel;
using DocumentPacker.Models;
using DocumentPacker.Parts.Main.EncryptPart.Translations;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;
using Libs.Wpf.ViewModels;

/// <summary>
///     Describes the data that gets encrypted.
/// </summary>
internal class EncryptDataViewModel : ViewModelBase, IEncryptDataViewModel
{
    /// <summary>
    ///     The description.
    /// </summary>
    private TranslatableAndValidable<string>? description;

    /// <summary>
    ///     The items.
    /// </summary>
    private ObservableCollection<IEncryptDataItemViewModel>? items;

    /// <summary>
    ///     The rsa public key.
    /// </summary>
    private string rsaPublicKey;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EncryptDataViewModel" /> class.
    /// </summary>
    public EncryptDataViewModel(ConfigurationModel configurationModel, ICommandFactory commandFactory)
    {
        this.Description = new TranslatableAndValidable<string>(
            configurationModel.Description,
            null,
            false,
            EncryptPartTranslation.ResourceManager,
            nameof(EncryptPartTranslation.DescriptionLabel));

        this.Items = new ObservableCollection<IEncryptDataItemViewModel>(
            configurationModel.ConfigurationItems.Select(
                item => new EncryptDataItemViewModel(
                    item,
                    commandFactory)));

        this.rsaPublicKey = configurationModel.RsaPublicKey;
    }

    /// <summary>
    ///     Gets or sets the description.
    /// </summary>
    public TranslatableAndValidable<string>? Description
    {
        get => this.description;
        set =>
            this.SetField(
                ref this.description,
                value);
    }

    /// <summary>
    ///     Gets or sets the items.
    /// </summary>
    public ObservableCollection<IEncryptDataItemViewModel>? Items
    {
        get => this.items;
        set =>
            this.SetField(
                ref this.items,
                value);
    }

    /// <summary>
    ///     Gets or sets the rsa public key.
    /// </summary>
    public string RsaPublicKey
    {
        get => this.rsaPublicKey;
        set =>
            this.SetField(
                ref this.rsaPublicKey,
                value);
    }

    /// <summary>
    ///     Validates the view model.
    /// </summary>
    /// <returns><c>True</c> if the validation succeeds; <c>false</c> otherwise.</returns>
    public bool Validate()
    {
        return this.Items?.Any() == true && this.Items.Select(item => item.Validate()).ToArray().All(x => x);
    }
}
