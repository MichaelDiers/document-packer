namespace DocumentPacker.Parts.EncryptStartUpPart.ViewModel;

using System.Windows.Input;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.EncryptStartUpPart.Contracts;

/// <summary>
///     The encrypt startup view model.
/// </summary>
/// <summary>
///     Describes link data on the startup view.
/// </summary>
/// <param name="name">The name of the link.</param>
/// <param name="description">The description of the link</param>
/// <param name="followLinkCommand">The command to follow the link.</param>
/// <seealso cref="DocumentPacker.Parts.ViewModel.PartViewModel" />
internal class EncryptStartUpLinkViewModel(string name, string description, ICommand followLinkCommand) : BaseViewModel,
    IEncryptStartUpLinkViewModel
{
    /// <summary>
    ///     The description of the link.
    /// </summary>
    private string description = description;

    /// <summary>
    ///     The command to follow the link.
    /// </summary>
    private ICommand followLinkCommand = followLinkCommand;

    /// <summary>
    ///     The name of the link.
    /// </summary>
    private string name = name;

    /// <summary>
    ///     Gets or sets the description of the link.
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
    ///     Gets or sets the command to follow the link.
    /// </summary>
    public ICommand FollowLinkCommand
    {
        get => this.followLinkCommand;
        set =>
            this.SetField(
                ref this.followLinkCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the name of the link.
    /// </summary>
    public string Name
    {
        get => this.name;
        set =>
            this.SetField(
                ref this.name,
                value);
    }
}
