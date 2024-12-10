namespace DocumentPacker.Parts.StartUpPart.Contracts;

using System.ComponentModel;
using System.Windows.Input;

/// <summary>
///     Describes link data on the startup view.
/// </summary>
public interface IStartUpLinkViewModel : INotifyPropertyChanged
{
    /// <summary>
    ///     Gets the description of the link.
    /// </summary>
    string Description { get; }

    /// <summary>
    ///     Gets the command to follow the link.
    /// </summary>
    ICommand FollowLinkCommand { get; }

    /// <summary>
    ///     Gets the name of the link.
    /// </summary>
    string Name { get; }
}
