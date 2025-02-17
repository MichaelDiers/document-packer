namespace DocumentPacker.Resources;

using System.Globalization;
using DocumentPacker.Parts.Header.HeaderPart;
using DocumentPacker.Parts.Header.Links.BackLinkPart;
using DocumentPacker.Parts.Header.Links.ChangeLanguageLinkPart;
using DocumentPacker.Parts.Main.ChangeLanguagePart;
using DocumentPacker.Parts.Main.CreateConfigurationPart;
using DocumentPacker.Parts.Main.DecryptPart;
using DocumentPacker.Parts.Main.EncryptPart;
using DocumentPacker.Parts.Main.FeaturesPart;
using DocumentPacker.Parts.WindowPart;

internal static class Translator
{
    public static void ChangeCultureInfo(CultureInfo cultureInfo)
    {
        // Todo: simplify
        Translation.Culture = cultureInfo;
        HeaderPartTranslation.Culture = cultureInfo;
        BackLinkPartTranslation.Culture = cultureInfo;
        ChangeLanguageLinkPartTranslation.Culture = cultureInfo;
        ChangeLanguagePartTranslation.Culture = cultureInfo;
        EncryptPartTranslation.Culture = cultureInfo;
        DecryptPartTranslation.Culture = cultureInfo;
        CreateConfigurationPartTranslation.Culture = cultureInfo;
        FeaturesPartTranslation.Culture = cultureInfo;
        WindowPartTranslation.Culture = cultureInfo;
    }
}
