namespace DocumentPacker.Parts.Header.Links.ChangeLanguageLinkPart;

using System.Windows.Input;
using System.Windows.Media.Imaging;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using Libs.Wpf.Commands;
using Libs.Wpf.ViewModels;

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
            new BitmapImage(
                new Uri(
                    "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_language.png",
                    UriKind.Absolute)),
            ChangeLanguageLinkPartTranslation.ResourceManager,
            toolTipResourceKey: nameof(ChangeLanguageLinkPartTranslation.ChangeLanguage));
    }

    /// <summary>
    ///     Gets the command to show the change language view.
    /// </summary>
    public TranslatableButton<ICommand> RequestChangeLanguageViewCommand { get; }
}
