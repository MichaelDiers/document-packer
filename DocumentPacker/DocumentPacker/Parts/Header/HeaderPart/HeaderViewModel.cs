namespace DocumentPacker.Parts.Header.HeaderPart;

using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;

/// <summary>
///     View model of the <see cref="HeaderView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class HeaderViewModel : ApplicationBaseViewModel
{
    /// <summary>
    ///     The navigation bar.
    /// </summary>
    private object? navigationBar;

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
