namespace DocumentPacker.Commands;

using Libs.Wpf.Localization;

/// <summary>
///     Synchronizes the execution of commands.
/// </summary>
internal class CommandSync : ICommandSync
{
    /// <summary>
    ///     A <see cref="Lock" /> to synchronize the access to <see cref="numberOfActiveCommands" />.
    /// </summary>
    private readonly Lock lockObject = new();

    /// <summary>
    ///     The number of active commands.
    /// </summary>
    private int numberOfActiveCommands;

    /// <summary>
    ///     Gets a value that indicates if a command is running (<c>true</c>).
    /// </summary>
    public bool IsCommandActive => this.numberOfActiveCommands != 0;

    /// <summary>
    ///     Raised if <see cref="ICommandSync.IsCommandActive" /> changed.
    /// </summary>
    public event EventHandler<CommandSyncChangedEventArgs>? CommandSyncChanged;

    /// <summary>
    ///     Requests to start a new command.
    /// </summary>
    /// <param name="force">Indicates that <see cref="Enter" /> should succeed even if a command is active.</param>
    /// <param name="translatableCancellableButton">The data of the cancellable command.</param>
    /// <returns><c>True</c> if the command is allowed to start; <c>false</c> otherwise.</returns>
    public bool Enter(bool force = false, TranslatableCancellableButton? translatableCancellableButton = null)
    {
        if (this.IsCommandActive && !force)
        {
            return false;
        }

        lock (this.lockObject)
        {
            if (this.numberOfActiveCommands != 0 && !force)
            {
                return false;
            }

            this.numberOfActiveCommands++;
            if (this.numberOfActiveCommands == 1)
            {
                this.CommandSyncChanged?.Invoke(
                    this,
                    new CommandSyncChangedEventArgs(
                        true,
                        translatableCancellableButton));
            }

            return true;
        }
    }

    /// <summary>
    ///     Indicates that a command is terminated.
    /// </summary>
    public void Exit()
    {
        lock (this.lockObject)
        {
            this.numberOfActiveCommands--;
            if (this.numberOfActiveCommands == 0)
            {
                this.CommandSyncChanged?.Invoke(
                    this,
                    new CommandSyncChangedEventArgs(
                        false,
                        null));
            }
        }
    }
}
