namespace DocumentPacker.Parts2.Main.ChangeLanguagePart;

using System.Globalization;

/// <summary>
///     Describes a supported language of the application.
/// </summary>
internal class ChangeLanguageElement(
    string language,
    CultureInfo cultureInfo,
    string iconPath,
    bool isCurrentLanguage
)
{
    /// <summary>
    ///     Gets the culture information of the language.
    /// </summary>
    public CultureInfo CultureInfo { get; } = cultureInfo;

    /// <summary>
    ///     Gets the path to the icon that identifies the language.
    /// </summary>
    public string IconPath { get; } = iconPath;

    /// <summary>
    ///     Gets a value indicating whether this instance is the current language.
    /// </summary>
    public bool IsCurrentLanguage { get; } = isCurrentLanguage;

    /// <summary>
    ///     Gets the language text in its translation.
    /// </summary>
    public string Language { get; } = language;
}
