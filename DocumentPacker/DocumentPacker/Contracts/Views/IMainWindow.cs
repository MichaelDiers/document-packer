namespace DocumentPacker.Contracts.Views;

/// <summary>
///     Describes the main window.
/// </summary>
public interface IMainWindow
{
    /// <summary>
    ///     Show the window
    /// </summary>
    /// <remarks>
    ///     Calling Show on window is the same as setting the
    ///     Visibility property to Visibility.Visible.
    /// </remarks>
    void Show();
}
