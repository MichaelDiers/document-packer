namespace DocumentPacker.Parts2.Main.ChangeLanguagePart;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using DocumentPacker.Resources;

/// <summary>
///     The view model of <see cref="ChangeLanguageView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationViewModel" />
internal class ChangeLanguageViewModel : ApplicationViewModel
{
    /// <summary>
    ///     The description text.
    /// </summary>
    private string description = Translation.ChangeLanguagePartDescription;

    /// <summary>
    ///     The headline that is displayed in the view.
    /// </summary>
    private string headline = Translation.ChangeLanguagePartHeadline;

    /// <summary>
    ///     The supported languages.
    /// </summary>
    private ObservableCollection<ChangeLanguageElement> languages = new(
        new[]
        {
            new ChangeLanguageElement(
                Translation.ResourceManager.GetString(
                    nameof(Translation.LanguageName),
                    CultureInfo.InvariantCulture)!,
                CultureInfo.InvariantCulture,
                "../../../Assets/92402_kingdom_united_icon.png",
                Translation.Culture is not null && Translation.Culture.Equals(CultureInfo.InvariantCulture)),
            new ChangeLanguageElement(
                Translation.ResourceManager.GetString(
                    nameof(Translation.LanguageName),
                    new CultureInfo("de"))!,
                new CultureInfo("de"),
                "../../../Assets/92094_germany_icon.png",
                Translation.Culture is not null && Translation.Culture.TwoLetterISOLanguageName == "de")
        });

    /// <summary>
    ///     Gets the change language command.
    /// </summary>
    public ICommand ChangeLanguageCommand =>
        new SyncCommand(
            obj => obj is ChangeLanguageElement {IsCurrentLanguage: false},
            obj =>
            {
                // obj is not of the expected type
                if (obj is not ChangeLanguageElement changeLanguageElement)
                {
                    return;
                }

                // store the current culture and set the text to the new culture
                var current = Translation.Culture;
                Translation.Culture = changeLanguageElement.CultureInfo;

                // in order to change the language an application restart is required.
                // ask the user if the application should be restarted now
                var messageBoxResult = MessageBox.Show(
                    Translation.BackLinkPartRestartRequest,
                    Translation.BackLinkPartRestartCaption,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.No);

                // reset the culture
                Translation.Culture = current;

                // request the restart of the application
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    this.InvokeShowViewRequested(
                        this,
                        new ShowViewRequestedEventArgs(ApplicationElementPart.Window)
                        {
                            Data = changeLanguageElement.CultureInfo
                        });
                }
            });

    /// <summary>
    ///     Gets or sets the description text.
    /// </summary>
    public string Description
    {
        get => this.description;
        set =>
            this.SetField(
                ref this.description,
                value);
    }

    /// <summary>
    ///     Gets or sets the headline that is displayed in the view.
    /// </summary>
    public string Headline
    {
        get => this.headline;
        set =>
            this.SetField(
                ref this.headline,
                value);
    }

    /// <summary>
    ///     Gets or sets the supported languages.
    /// </summary>
    public ObservableCollection<ChangeLanguageElement> Languages
    {
        get => this.languages;
        set =>
            this.SetField(
                ref this.languages,
                value);
    }
}
