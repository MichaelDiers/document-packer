namespace DocumentPacker.Parts.Header.HeaderPart;

using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using DocumentPacker.Resources;

/// <summary>
///     View model of the <see cref="HeaderView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationViewModel" />
internal class HeaderViewModel : ApplicationViewModel
{
    /// <summary>
    ///     The headline.
    /// </summary>
    private string headline = Translation.HeaderPartHeadline;

    /// <summary>
    ///     The navigation bar.
    /// </summary>
    private object? navigationBar;

    /// <summary>
    ///     Gets or sets the headline.
    /// </summary>
    public string Headline
    {
        get => this.headline;
        set =>
            this.SetField(
                ref this.headline,
                value);
    }

    /// <summary>
    ///     Gets or sets the navigation bar.
    /// </summary>
    [ApplicationPart(ApplicationElementPart.NavigationBar)]
    public object? NavigationBar
    {
        get => this.navigationBar;
        set =>
            this.SetField(
                ref this.navigationBar,
                value);
    }
}
