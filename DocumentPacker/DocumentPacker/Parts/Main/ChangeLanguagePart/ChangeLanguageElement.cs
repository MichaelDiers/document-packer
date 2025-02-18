namespace DocumentPacker.Parts.Main.ChangeLanguagePart;

using System.Globalization;
using Libs.Wpf.ViewModels;

/// <summary>
///     Describes a supported language of the application.
/// </summary>
/// <param name="language">The language text in its translation.</param>
/// <param name="cultureInfo">The culture information of the language.</param>
/// <param name="iconPath">The path to the icon that identifies the language.</param>
/// <param name="isCurrentLanguage">A value that indicates weather this element represents the active application language.</param>
internal class ChangeLanguageElement(
    string language,
    CultureInfo cultureInfo,
    string iconPath,
    bool isCurrentLanguage
) : ViewModelBase
{
    /// <summary>
    ///     A value that indicates weather this element represents the active application language.
    /// </summary>
    private bool isCurrentLanguage = isCurrentLanguage;

    /// <summary>
    ///     Gets the culture information of the language.
    /// </summary>
    public CultureInfo CultureInfo { get; } = cultureInfo;

    /// <summary>
    ///     Gets the path to the icon that identifies the language.
    /// </summary>
    public string IconPath { get; } = iconPath;

    /// <summary>
    ///     Gets or sets a value that indicates weather this element represents the active application language.
    /// </summary>
    public bool IsCurrentLanguage
    {
        get => this.isCurrentLanguage;
        set =>
            this.SetField(
                ref this.isCurrentLanguage,
                value);
    }

    /// <summary>
    ///     Gets the language text in its translation.
    /// </summary>
    public string Language { get; } = language;
}
