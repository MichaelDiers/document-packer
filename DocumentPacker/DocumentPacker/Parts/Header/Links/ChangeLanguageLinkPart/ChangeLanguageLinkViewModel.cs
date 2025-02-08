namespace DocumentPacker.Parts.Header.Links.ChangeLanguageLinkPart;

using System.Windows.Input;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using DocumentPacker.Resources;

/// <summary>
///     View model of the <see cref="ChangeLanguageLinkView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationViewModel" />
internal class ChangeLanguageLinkViewModel : ApplicationViewModel
{
    /// <summary>
    ///     The text of the command to show the change language view.
    /// </summary>
    private string commandText = Translation.BackLinkPartChangeLanguage;

    /// <summary>
    ///     Gets or sets the text of the command to show the change language view.
    /// </summary>
    public string CommandText
    {
        get => this.commandText;
        set =>
            this.SetField(
                ref this.commandText,
                value);
    }

    /// <summary>
    ///     Gets the command to show the change language view.
    /// </summary>
    public ICommand RequestChangeLanguageViewCommand =>
        new SyncCommand(
            _ => true,
            _ => this.InvokeShowViewRequested(
                this,
                new ShowViewRequestedEventArgs(ApplicationElementPart.ChangeLanguage)));
}
