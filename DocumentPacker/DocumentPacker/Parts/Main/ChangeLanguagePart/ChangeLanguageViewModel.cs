namespace DocumentPacker.Parts.Main.ChangeLanguagePart;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using DocumentPacker.Mvvm;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;

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
                TranslationSource.Instance.CurrentCulture.Equals(CultureInfo.InvariantCulture)),
            new ChangeLanguageElement(
                ChangeLanguagePartTranslation.ResourceManager.GetString(
                    nameof(ChangeLanguagePartTranslation.LanguageName),
                    new CultureInfo("de"))!,
                new CultureInfo("de"),
                "../../../Assets/92094_germany_icon.png",
                TranslationSource.Instance.CurrentCulture.TwoLetterISOLanguageName == "de")
        });

    /// <summary>
    ///     Gets the change language command.
    /// </summary>
    public ICommand ChangeLanguageCommand =>
        commandFactory.CreateSyncCommand<ChangeLanguageElement>(
            changeLanguageElement => changeLanguageElement?.IsCurrentLanguage == false,
            changeLanguageElement =>
            {
                // obj is not of the expected type
                if (changeLanguageElement is null)
                {
                    return;
                }

                TranslationSource.Instance.CurrentCulture = changeLanguageElement.CultureInfo;
                foreach (var languageElement in this.Languages)
                {
                    languageElement.IsCurrentLanguage =
                        TranslationSource.Instance.CurrentCulture.TwoLetterISOLanguageName ==
                        languageElement.CultureInfo.TwoLetterISOLanguageName;
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
