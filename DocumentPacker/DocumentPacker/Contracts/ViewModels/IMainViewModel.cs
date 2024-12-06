namespace DocumentPacker.Contracts.ViewModels;

using System.Windows.Input;
using DocumentPacker.Contracts.ViewModels.SubViewModels;

/// <summary>
///     Describes the main view model.
/// </summary>
public interface IMainViewModel
{
    /// <summary>
    ///     Gets the close fatal error message command.
    /// </summary>
    ICommand CloseFatalErrorMessageCommand { get; }

    /// <summary>
    ///     Gets or sets the fatal error message text.
    /// </summary>
    string? FatalErrorMessage { get; set; }

    /// <summary>
    ///     Gets the home command.
    /// </summary>
    ICommand HomeCommand { get; }

    /// <summary>
    ///     Gets or sets the sub view model.
    /// </summary>
    ISubViewModel SubViewModel { get; }

    /// <summary>
    ///     Gets or sets the title.
    /// </summary>
    string Title { get; set; }

    /// <summary>
    ///     Gets or sets the version of the application.
    /// </summary>
    string Version { get; set; }
}
