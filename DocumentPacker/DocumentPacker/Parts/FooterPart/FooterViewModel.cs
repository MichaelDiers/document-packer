namespace DocumentPacker.Parts.FooterPart;

using DocumentPacker.Mvvm;
using Libs.Wpf.Localization;

/// <summary>
///     View model of the <see cref="FooterView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class FooterViewModel : ApplicationBaseViewModel
{
    /// <summary>
    ///     The version information of the application.
    /// </summary>
    private Translatable version = new(
        FooterPartTranslation.ResourceManager,
        nameof(FooterPartTranslation.Version));

    /// <summary>
    ///     Gets or sets the version information.
    /// </summary>
    public Translatable Version
    {
        get => this.version;
        set =>
            this.SetField(
                ref this.version,
                value);
    }
}
