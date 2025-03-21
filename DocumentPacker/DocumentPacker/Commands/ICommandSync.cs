﻿namespace DocumentPacker.Commands;

using Libs.Wpf.Localization;

/// <summary>
///     Synchronizes the execution of commands.
/// </summary>
public interface ICommandSync
{
    /// <summary>
    ///     Gets a value that indicates if a command is running (<c>true</c>).
    /// </summary>
    bool IsCommandActive { get; }

    /// <summary>
    ///     Raised if <see cref="IsCommandActive" /> changed.
    /// </summary>
    event EventHandler<CommandSyncChangedEventArgs>? CommandSyncChanged;

    /// <summary>
    ///     Requests to start a new command.
    /// </summary>
    /// <param name="force">Indicates that <see cref="Enter" /> should succeed even if a command is active.</param>
    /// <param name="translatableCancellableButton">The data of the cancellable command.</param>
    /// <returns><c>True</c> if the command is allowed to start; <c>false</c> otherwise.</returns>
    bool Enter(bool force = false, TranslatableCancellableButton? translatableCancellableButton = null);

    /// <summary>
    ///     Indicates that a command is terminated.
    /// </summary>
    void Exit();
}
