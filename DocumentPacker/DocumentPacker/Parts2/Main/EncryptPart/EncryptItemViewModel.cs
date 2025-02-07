namespace DocumentPacker.Parts2.Main.EncryptPart;

using System.Collections.ObjectModel;
using System.IO;
using DocumentPacker.Mvvm;
using DocumentPacker.Resources;

internal class EncryptItemViewModel : BaseViewModel, IHandleDragAndDrop
{
    /// <summary>
    ///     The id of the archive entry.
    /// </summary>
    private string archiveId = string.Empty;

    /// <summary>
    ///     The archive id extension of the archive entry.
    /// </summary>
    private string archiveIdExtension = string.Empty;

    /// <summary>
    ///     The description of the element.
    /// </summary>
    private string description = string.Empty;

    /// <summary>
    ///     The type of the encrypt item.
    /// </summary>
    private EncryptItemType encryptItemType = EncryptItemType.None;

    /// <summary>
    ///     The supported encrypt item types.
    /// </summary>
    private ObservableCollection<EncryptItemType> encryptItemTypes = new(Enum.GetValues<EncryptItemType>());

    /// <summary>
    ///     A value that indicates weather the expander is expanded.
    /// </summary>
    private bool isExpanded = true;

    /// <summary>
    ///     The value indicating whether the item is required or optional.
    /// </summary>
    private bool isRequired = true;

    /// <summary>
    ///     The value of the encrypt item.
    /// </summary>
    private string value = string.Empty;

    /// <summary>
    ///     Gets or sets the id of the archive entry.
    /// </summary>
    public string ArchiveId
    {
        get => this.archiveId;
        set =>
            this.SetField(
                ref this.archiveId,
                value);
    }

    /// <summary>
    ///     Gets or sets the id extension of the archive entry.
    /// </summary>
    public string ArchiveIdExtension
    {
        get => this.archiveIdExtension;
        set =>
            this.SetField(
                ref this.archiveIdExtension,
                value);
    }

    /// <summary>
    ///     Gets or sets the description of the element.
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
    ///     Gets or sets the type of the encrypt item.
    /// </summary>
    public EncryptItemType EncryptItemType
    {
        get => this.encryptItemType;
        set
        {
            if (value != EncryptItemType.None && this.encryptItemType == EncryptItemType.None)
            {
                this.EncryptItemTypes.Remove(EncryptItemType.None);
            }

            this.SetField(
                ref this.encryptItemType,
                value,
                [() => this.ValidateValue(this.Value)]);

            if (this.encryptItemType == EncryptItemType.Text)
            {
                this.ArchiveIdExtension = ".txt";
            }
            else if (this.encryptItemType == EncryptItemType.File)
            {
                if (File.Exists(this.Value))
                {
                    this.ArchiveIdExtension = Path.GetExtension(this.Value);
                    if (string.IsNullOrWhiteSpace(this.Value))
                    {
                        this.ArchiveId = Path.GetFileNameWithoutExtension(this.Value);
                    }
                }
                else
                {
                    this.ArchiveIdExtension = string.Empty;
                }
            }
        }
    }

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
    ///     Gets or sets the value indicating whether the item is required or optional.
    /// </summary>
    public bool IsRequired
    {
        get => this.isRequired;
        set =>
            this.SetField(
                ref this.isRequired,
                value,
                [() => this.ValidateValue(this.Value)]);
    }

    /// <summary>
    ///     Gets or sets the value of the encrypt item.
    /// </summary>
    public string Value
    {
        get => this.value;
        set
        {
            this.SetField(
                ref this.value,
                value,
                [() => this.ValidateValue(value)]);

            if (this.EncryptItemType == EncryptItemType.File && File.Exists(this.Value))
            {
                this.ArchiveIdExtension = Path.GetExtension(this.Value);
                this.ArchiveId = Path.GetFileNameWithoutExtension(this.Value);
            }
        }
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

    private (string propertyName, IEnumerable<string> errors) ValidateValue(string newValue)
    {
        var propertyName = nameof(EncryptItemViewModel.Value);

        if (string.IsNullOrWhiteSpace(newValue))
        {
            if (this.isRequired)
            {
                switch (this.EncryptItemType)
                {
                    case EncryptItemType.File:
                        return (propertyName, [Translation.EncryptPartFileIsRequired]);
                    case EncryptItemType.Text:
                        return (propertyName, [Translation.EncryptPartTextIsRequired]);
                    default:
                        return (propertyName, []);
                }
            }
        }

        if (this.EncryptItemType == EncryptItemType.File && !File.Exists(newValue))
        {
            return (propertyName, [Translation.EncryptPartFileDoesNotExists]);
        }

        return (propertyName, []);
    }
}
