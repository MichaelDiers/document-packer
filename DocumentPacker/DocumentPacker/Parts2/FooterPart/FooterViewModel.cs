namespace DocumentPacker.Parts2.FooterPart;

using DocumentPacker.Mvvm;
using DocumentPacker.Resources;

/// <summary>
///     View model of the <see cref="FooterView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationViewModel" />
internal class FooterViewModel : ApplicationViewModel
{
    /// <summary>
    ///     The version information of the application.
    /// </summary>
    private string version = string.Format(
        Translation.FooterPartVersion,
        Translation.DocumentPacker,
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
