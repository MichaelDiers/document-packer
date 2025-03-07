namespace DocumentPacker.Parts.Main.ChangeLanguagePart;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DocumentPacker.Mvvm;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;
using Libs.Wpf.ViewModels;

/// <summary>
///     The view model of <see cref="ChangeLanguageView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class ChangeLanguageViewModel : ApplicationBaseViewModel
{
    /// <summary>
    ///     The supported languages.
    /// </summary>
    private ObservableCollection<TranslatableButton<ICommand>> languages;

    /// <summary>
    ///     The description of the view.
    /// </summary>
    private Translatable viewDescription = new(
        ChangeLanguagePartTranslation.ResourceManager,
        nameof(ChangeLanguagePartTranslation.Description));

    /// <summary>
    ///     The headline of the view.
    /// </summary>
    private Translatable viewHeadline = new(
        ChangeLanguagePartTranslation.ResourceManager,
        nameof(ChangeLanguagePartTranslation.Headline));

    /// <summary>
    ///     Initializes a new instance of the <see cref="ChangeLanguageViewModel" /> class.
    /// </summary>
    public ChangeLanguageViewModel(ICommandFactory commandFactory)
    {
        this.languages = new ObservableCollection<TranslatableButton<ICommand>>(
            new[]
            {
                new TranslatableButton<ICommand>(
                    commandFactory.CreateSyncCommand(
                        _ => !TranslationSource.Instance.CurrentCulture.Equals(CultureInfo.InvariantCulture),
                        _ => TranslationSource.Instance.CurrentCulture = CultureInfo.InvariantCulture),
                    new BitmapImage(
                        new Uri(
                            "pack://application:,,,/DocumentPacker;component/Assets/92402_kingdom_united_icon.png",
                            UriKind.Absolute)),
                    ChangeLanguagePartTranslation.ResourceManager,
                    nameof(ChangeLanguagePartTranslation.LanguageNameUK)),
                new TranslatableButton<ICommand>(
                    commandFactory.CreateSyncCommand(
                        _ => TranslationSource.Instance.CurrentCulture.TwoLetterISOLanguageName != "de",
                        _ => TranslationSource.Instance.CurrentCulture = new CultureInfo("de")),
                    new BitmapImage(
                        new Uri(
                            "pack://application:,,,/DocumentPacker;component/Assets/92094_germany_icon.png",
                            UriKind.Absolute)),
                    ChangeLanguagePartTranslation.ResourceManager,
                    nameof(ChangeLanguagePartTranslation.LanguageNameDE))
            });
    }

    /// <summary>
    ///     Gets or sets the supported languages.
    /// </summary>
    public ObservableCollection<TranslatableButton<ICommand>> Languages
    {
        get => this.languages;
        set =>
            this.SetField(
                ref this.languages,
                value);
    }

    /// <summary>
    ///     Gets or sets the description of the view.
    /// </summary>
    public Translatable ViewDescription
    {
        get => this.viewDescription;
        set =>
            this.SetField(
                ref this.viewDescription,
                value);
    }

    /// <summary>
    ///     Gets or sets the headline of the view.
    /// </summary>
    public Translatable ViewHeadline
    {
        get => this.viewHeadline;
        set =>
            this.SetField(
                ref this.viewHeadline,
                value);
    }
}
