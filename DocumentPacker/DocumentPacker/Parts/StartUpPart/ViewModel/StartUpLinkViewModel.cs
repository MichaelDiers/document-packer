namespace DocumentPacker.Parts.StartUpPart.ViewModel;

using System.Windows.Input;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.StartUpPart.Contracts;

/// <summary>
///     Describes link data on the startup view.
/// </summary>
/// <param name="name">The name of the link.</param>
/// <param name="description">The description of the link</param>
/// <param name="followLinkCommand">The command to follow the link.</param>
/// <seealso cref="DocumentPacker.Mvvm.BaseViewModel" />
/// <seealso cref="IStartUpLinkViewModel" />
internal class StartUpLinkViewModel(string name, string description, ICommand followLinkCommand) : BaseViewModel,
    IStartUpLinkViewModel
{
    /// <summary>
    ///     Gets the description of the link.
    /// </summary>
    public string Description { get; } = description;

    /// <summary>
    ///     Gets the command to follow the link.
    /// </summary>
    public ICommand FollowLinkCommand { get; } = followLinkCommand;

    /// <summary>
    ///     Gets the name of the link.
    /// </summary>
    public string Name { get; } = name;
}
