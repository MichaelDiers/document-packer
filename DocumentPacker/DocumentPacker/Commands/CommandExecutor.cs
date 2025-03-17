namespace DocumentPacker.Commands;

using System.Windows;
using Libs.Wpf.Localization;
using Libs.Wpf.ViewModels;

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

    public static void PostExecute(Task<(bool success, string? message)> task)
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
                MessageBox.Show(
                    message,
                    caption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(
                    message,
                    string.Empty,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                string.Empty,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
