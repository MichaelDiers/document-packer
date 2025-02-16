namespace DocumentPacker.Resources;

using System.Globalization;
using DocumentPacker.Parts.Header.HeaderPart;
using DocumentPacker.Parts.Header.Links.BackLinkPart;
using DocumentPacker.Parts.Header.Links.ChangeLanguageLinkPart;

internal static class Translator
{
    public static void ChangeCultureInfo(CultureInfo cultureInfo)
    {
        // Todo: simplify
        Translation.Culture = cultureInfo;
        HeaderPartTranslation.Culture = cultureInfo;
        BackLinkPartTranslation.Culture = cultureInfo;
        ChangeLanguagePartTranslation.Culture = cultureInfo;
    }
}
