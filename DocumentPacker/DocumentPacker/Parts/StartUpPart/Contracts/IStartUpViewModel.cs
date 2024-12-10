namespace DocumentPacker.Parts.StartUpPart.Contracts;

using DocumentPacker.Parts.Contracts;

/// <summary>
///     The view model data that are displayed at application startup.
/// </summary>
public interface IStartUpViewModel : IPartViewModel
{
    /// <summary>
    ///     Gets the links that can be followed from the startup view.
    /// </summary>
    IEnumerable<IStartUpLinkViewModel> Links { get; }
}
