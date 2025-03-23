namespace DocumentPacker.Parts.Header.Links.ChangeLanguageLinkPart;

using System.Windows.Input;
using DocumentPacker.EventHandling;
using DocumentPacker.Extensions;
using DocumentPacker.Mvvm;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;

/// <summary>
///     View model of the <see cref="ChangeLanguageLinkView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class ChangeLanguageLinkViewModel : ApplicationBaseViewModel
{
    /// <summary>
    ///     View model of the <see cref="ChangeLanguageLinkView" />.
    /// </summary>
    /// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
    public ChangeLanguageLinkViewModel(ICommandFactory commandFactory)
    {
        this.RequestChangeLanguageViewCommand = new TranslatableButton<ICommand>(
            commandFactory.CreateSyncCommand(
                _ => true,
                _ => this.InvokeShowViewRequested(
                    this,
                    new ShowViewRequestedEventArgs(ApplicationElementPart.ChangeLanguage))),
            "material_symbol_language.png".ToPackImage(),
            ChangeLanguageLinkPartTranslation.ResourceManager,
            toolTipResourceKey: nameof(ChangeLanguageLinkPartTranslation.ChangeLanguage));
    }

    /// <summary>
    ///     Gets the command to show the change language view.
    /// </summary>
    public TranslatableButton<ICommand> RequestChangeLanguageViewCommand { get; }
}
