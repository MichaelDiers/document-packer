namespace DocumentPacker.Parts2.Main.EncryptPart;

using System.Collections.ObjectModel;
using System.Windows.Input;
using DocumentPacker.Mvvm;
using DocumentPacker.Resources;
using Microsoft.Win32;

/// <summary>
///     The view model of <see cref="EncryptView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationViewModel" />
internal class EncryptViewModel : ApplicationViewModel
{
    /// <summary>
    ///     The description text.
    /// </summary>
    private string description = Translation.FeaturesPartEncryptDescription;

    /// <summary>
    ///     The encrypt items.
    /// </summary>
    private ObservableCollection<EncryptItemViewModel> encryptItems = new(
        [new EncryptItemViewModel {EncryptItemType = EncryptItemType.File}]);

    /// <summary>
    ///     The headline text.
    /// </summary>
    private string headline = Translation.FeaturesPartEncryptHeadline;

    /// <summary>
    ///     Gets the add encrypt item command.
    /// </summary>
    public ICommand AddEncryptItemCommand =>
        new SyncCommand(
            _ => true,
            _ => this.EncryptItems.Add(new EncryptItemViewModel()));

    /// <summary>
    ///     Gets the attach-file command.
    /// </summary>
    public ICommand AttachFileCommand =>
        new SyncCommand(
            obj => obj is EncryptItemViewModel,
            obj =>
            {
                if (obj is not EncryptItemViewModel encryptItemViewModel)
                {
                    return;
                }

                var dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    encryptItemViewModel.Value = dialog.FileName;
                }
            });

    /// <summary>
    ///     Gets the delete-encrypt item command.
    /// </summary>
    public ICommand DeleteEncryptItemCommand =>
        new SyncCommand(
            _ => true,
            obj =>
            {
                if (obj is not EncryptItemViewModel encryptItem)
                {
                    throw new ArgumentException(
                        nameof(EncryptItemViewModel),
                        nameof(obj));
                }

                this.EncryptItems.Remove(encryptItem);
            });

    /// <summary>
    ///     Gets or sets the description text.
    /// </summary>
    public string Description
    {
        get => this.description;
        set =>
            this.SetField(
                ref this.description,
                value);
    }

    /// <summary>
    ///     Gets or sets the encrypt items.
    /// </summary>
    public ObservableCollection<EncryptItemViewModel> EncryptItems
    {
        get => this.encryptItems;
        set =>
            this.SetField(
                ref this.encryptItems,
                value);
    }

    /// <summary>
    ///     Gets or sets the headline text.
    /// </summary>
    public string Headline
    {
        get => this.headline;
        set =>
            this.SetField(
                ref this.headline,
                value);
    }
}
