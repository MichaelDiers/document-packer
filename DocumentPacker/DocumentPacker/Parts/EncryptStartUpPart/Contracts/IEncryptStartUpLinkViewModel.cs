namespace DocumentPacker.Parts.EncryptStartUpPart.Contracts;

using System.Windows.Input;

/// <summary>
///     The view model data that are displayed as the encryption startup link.
/// </summary>
internal interface IEncryptStartUpLinkViewModel
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
