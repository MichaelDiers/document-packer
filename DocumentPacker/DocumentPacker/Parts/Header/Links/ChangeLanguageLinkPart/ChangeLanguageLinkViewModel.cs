namespace DocumentPacker.Parts.Header.Links.ChangeLanguageLinkPart;

using System.Windows.Input;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using Libs.Wpf.Commands;

/// <summary>
///     View model of the <see cref="ChangeLanguageLinkView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class ChangeLanguageLinkViewModel(ICommandFactory commandFactory) : ApplicationBaseViewModel
{
    /// <summary>
    ///     Gets the command to show the change language view.
    /// </summary>
    public ICommand RequestChangeLanguageViewCommand =>
        commandFactory.CreateSyncCommand(
            _ => true,
            _ => this.InvokeShowViewRequested(
                this,
                new ShowViewRequestedEventArgs(ApplicationElementPart.ChangeLanguage)));
}
