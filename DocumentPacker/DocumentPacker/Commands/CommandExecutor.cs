namespace DocumentPacker.Commands;

using Libs.Wpf.Controls.CustomMessageBox;
using Libs.Wpf.Localization;

internal static class CommandExecutor
{
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
