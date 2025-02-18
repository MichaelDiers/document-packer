namespace DocumentPacker.Resources;

using System.Globalization;
using Libs.Wpf.Localization;

internal static class Translator
{
    public static void ChangeCultureInfo(CultureInfo cultureInfo)
    {
        TranslationSource.Instance.CurrentCulture = cultureInfo;
        // Todo: simplify
        /**
        HeaderPartTranslation.Culture = cultureInfo;
        BackLinkPartTranslation.Culture = cultureInfo;
        ChangeLanguageLinkPartTranslation.Culture = cultureInfo;
        ChangeLanguagePartTranslation.Culture = cultureInfo;
        EncryptPartTranslation.Culture = cultureInfo;
        DecryptPartTranslation.Culture = cultureInfo;
        CreateConfigurationPartTranslation.Culture = cultureInfo;
        FeaturesPartTranslation.Culture = cultureInfo;
        WindowPartTranslation.Culture = cultureInfo;
        FooterPartTranslation.Culture = cultureInfo;
        **/
    }
}
