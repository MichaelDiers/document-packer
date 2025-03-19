namespace DocumentPacker.Parts.Header.HeaderPart;

using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using Libs.Wpf.Localization;

/// <summary>
///     View model of the <see cref="HeaderView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class HeaderViewModel : ApplicationBaseViewModel
{
    /// <summary>
    ///     The headline.
    /// </summary>
    private Translatable headline = new(
        HeaderPartTranslation.ResourceManager,
        nameof(HeaderPartTranslation.Headline));

    /// <summary>
    ///     The navigation bar.
    /// </summary>
    private object? navigationBar;

    /// <summary>
    ///     Gets or sets the headline.
    /// </summary>
    public Translatable Headline
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
