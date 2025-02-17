namespace DocumentPacker.Parts.FooterPart;

using DocumentPacker.Mvvm;

/// <summary>
///     View model of the <see cref="FooterView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class FooterViewModel : ApplicationBaseViewModel
{
    /// <summary>
    ///     The version information of the application.
    /// </summary>
    private string version = string.Format(
        FooterPartTranslation.Version,
        FooterPartTranslation.DocumentPacker,
        "0.0.1");

    /// <summary>
    ///     Gets or sets the version information.
    /// </summary>
    public string Version
    {
        get => this.version;
        set =>
            this.SetField(
                ref this.version,
                value);
    }
}
