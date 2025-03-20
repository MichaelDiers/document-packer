namespace DocumentPacker.Commands;

using Libs.Wpf.Localization;

/// <summary>
///     The <see cref="EventArgs" /> of the <see cref="ICommandSync.CommandSyncChanged" />.
/// </summary>
/// <param name="isCommandActive">Indicates if a command is active.</param>
/// <param name="translatableCancellableButton">The optional data to cancel an active command.</param>
public class CommandSyncChangedEventArgs(
    bool isCommandActive,
    TranslatableCancellableButton? translatableCancellableButton
) : EventArgs
{
    /// <summary>
    ///     Gets a value that indicates if a command is active.
    /// </summary>
    public bool IsCommandActive => isCommandActive;

    /// <summary>
    ///     Gets the data to cancel an active command.
    /// </summary>
    public TranslatableCancellableButton? TranslatableCancellableButton => translatableCancellableButton;
}
