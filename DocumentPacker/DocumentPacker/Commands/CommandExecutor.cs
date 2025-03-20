namespace DocumentPacker.Commands;

using Libs.Wpf.Controls.CustomMessageBox;
using Libs.Wpf.Localization;

/// <summary>
///     A helper to execute commands.
/// </summary>
public static class CommandExecutor
{
    /// <summary>
    ///     Execute <paramref name="execute" /> and handles errors.
    /// </summary>
    /// <param name="validate">A validation function that is called before <paramref name="execute" />.</param>
    /// <param name="commandSync">The command sync that ensures that only one command is executed at the same time.</param>
    /// <param name="execute">The execute method of the command.</param>
    /// <param name="translatableCancellableButton">The cancel button to abort the execution.</param>
    /// <returns>A <see cref="Task{T}" /> whose indicates success and a message.</returns>
    public static async Task<(bool success, string? message)> Execute(
        Func<bool> validate,
        ICommandSync commandSync,
        Func<Task<(bool success, string? message)>> execute,
        TranslatableCancellableButton? translatableCancellableButton
    )
    {
        if (!validate())
        {
            return (false, null);
        }

        if (!commandSync.Enter(translatableCancellableButton: translatableCancellableButton))
        {
            return (false, CommandTranslations.ResourceManager.GetString(
                nameof(CommandTranslations.UnknownError),
                TranslationSource.Instance.CurrentCulture));
        }

        try
        {
            return await execute();
        }
        catch (Exception ex)
        {
            var message = CommandTranslations.ResourceManager.GetString(
                              nameof(CommandTranslations.UnknownException),
                              TranslationSource.Instance.CurrentCulture) ??
                          "{0}";
            return (false, string.Format(
                message,
                ex.Message));
        }
        finally
        {
            commandSync.Exit();
        }
    }

    /// <summary>
    ///     Executes the post execute method of a command and handles errors.
    /// </summary>
    /// <param name="task">The result <see cref="Task{T}" /> of the command execution.</param>
    /// <param name="messageBoxService">A service for showing a message box.</param>
    public static void PostExecute(Task<(bool success, string? message)> task, IMessageBoxService messageBoxService)
    {
        try
        {
            var (success, message) = task.Result;
            if (!success)
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    return;
                }

                var caption = CommandTranslations.ResourceManager.GetString(
                                  nameof(CommandTranslations.MessageBoxCaptionError),
                                  TranslationSource.Instance.CurrentCulture) ??
                              string.Empty;
                messageBoxService.Show(
                    message,
                    caption,
                    MessageBoxButtons.Ok,
                    MessageBoxButtons.Ok,
                    MessageBoxImage.Error);
            }
            else
            {
                messageBoxService.Show(
                    message ?? string.Empty,
                    string.Empty,
                    MessageBoxButtons.Ok,
                    MessageBoxButtons.Ok,
                    MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            messageBoxService.Show(
                ex.Message,
                string.Empty,
                MessageBoxButtons.Ok,
                MessageBoxButtons.Ok,
                MessageBoxImage.Error);
        }
    }
}
