namespace DocumentPacker.Parts.Main.ChangeLanguagePart;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using Libs.Wpf.Commands;

/// <summary>
///     The view model of <see cref="ChangeLanguageView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class ChangeLanguageViewModel(ICommandFactory commandFactory) : ApplicationBaseViewModel
{
    /// <summary>
    ///     The supported languages.
    /// </summary>
    private ObservableCollection<ChangeLanguageElement> languages = new(
        new[]
        {
            new ChangeLanguageElement(
                ChangeLanguagePartTranslation.ResourceManager.GetString(
                    nameof(ChangeLanguagePartTranslation.LanguageName),
                    CultureInfo.InvariantCulture)!,
                CultureInfo.InvariantCulture,
                "../../../Assets/92402_kingdom_united_icon.png",
                ChangeLanguagePartTranslation.Culture is not null &&
                ChangeLanguagePartTranslation.Culture.Equals(CultureInfo.InvariantCulture)),
            new ChangeLanguageElement(
                ChangeLanguagePartTranslation.ResourceManager.GetString(
                    nameof(ChangeLanguagePartTranslation.LanguageName),
                    new CultureInfo("de"))!,
                new CultureInfo("de"),
                "../../../Assets/92094_germany_icon.png",
                ChangeLanguagePartTranslation.Culture is not null &&
                ChangeLanguagePartTranslation.Culture.TwoLetterISOLanguageName == "de")
        });

    /// <summary>
    ///     Gets the change language command.
    /// </summary>
    public ICommand ChangeLanguageCommand =>
        commandFactory.CreateSyncCommand(
            obj => obj is ChangeLanguageElement {IsCurrentLanguage: false},
            obj =>
            {
                // obj is not of the expected type
                if (obj is not ChangeLanguageElement changeLanguageElement)
                {
                    return;
                }

                // store the current culture and set the text to the new culture
                var current = ChangeLanguagePartTranslation.Culture;
                ChangeLanguagePartTranslation.Culture = changeLanguageElement.CultureInfo;

                // in order to change the language an application restart is required.
                // ask the user if the application should be restarted now
                var messageBoxResult = MessageBox.Show(
                    ChangeLanguagePartTranslation.RestartMessageBoxText,
                    ChangeLanguagePartTranslation.RestartMessageBoxCaption,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.No);

                // reset the culture
                ChangeLanguagePartTranslation.Culture = current;

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
