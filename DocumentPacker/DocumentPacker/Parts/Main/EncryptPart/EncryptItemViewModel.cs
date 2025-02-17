namespace DocumentPacker.Parts.Main.EncryptPart;

using System.Collections.ObjectModel;
using System.IO;
using Libs.Wpf.ViewModels;

internal class EncryptItemViewModel : ValidatorViewModelBase, IHandleDragAndDrop
{
    /// <summary>
    ///     The supported encrypt item types.
    /// </summary>
    private ObservableCollection<EncryptItemType> encryptItemTypes = new(Enum.GetValues<EncryptItemType>());

    /// <summary>
    ///     A value that indicates weather the expander is expanded.
    /// </summary>
    private bool isExpanded = true;

    /// <summary>
    ///     The type of the encrypt item.
    /// </summary>
    private EncryptItemType? selectedEncryptItemType;

    /// <summary>
    ///     The value of the encrypt item.
    /// </summary>
    private string value = string.Empty;

    /// <summary>
    ///     Gets or sets the supported encrypt item types.
    /// </summary>
    public ObservableCollection<EncryptItemType> EncryptItemTypes
    {
        get => this.encryptItemTypes;
        set =>
            this.SetField(
                ref this.encryptItemTypes,
                value);
    }

    /// <summary>
    ///     Gets or sets a value that indicates weather the expander is expanded.
    /// </summary>
    public bool IsExpanded
    {
        get => this.isExpanded;
        set =>
            this.SetField(
                ref this.isExpanded,
                value);
    }

    /// <summary>
    ///     Gets or sets the type of the encrypt item.
    /// </summary>
    public EncryptItemType? SelectedEncryptItemType
    {
        get => this.selectedEncryptItemType;
        set =>
            this.SetField(
                ref this.selectedEncryptItemType,
                value);
    }

    /// <summary>
    ///     Gets or sets the value of the encrypt item.
    /// </summary>
    public string Value
    {
        get => this.value;
        set =>
            this.SetField(
                ref this.value,
                value);
    }

    public bool CanHandleDragAndDrop(IEnumerable<string> files)
    {
        var file = files.FirstOrDefault();
        return file != null && File.Exists(file);
    }

    public void HandleDragAndDrop(IEnumerable<string> files)
    {
        this.Value = files.FirstOrDefault() ?? string.Empty;
    }
}
