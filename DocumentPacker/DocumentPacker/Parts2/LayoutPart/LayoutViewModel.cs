namespace DocumentPacker.Parts2.LayoutPart;

using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;

/// <summary>
///     View model of the <see cref="LayoutView" />. Defines the layout of the main window.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationViewModel" />
internal class LayoutViewModel : ApplicationViewModel
{
    /// <summary>
    ///     The view that is displayed in the footer part.
    /// </summary>
    private object? footerView;

    /// <summary>
    ///     The view that is displayed in the header part.
    /// </summary>
    private object? headerView;

    /// <summary>
    ///     The view that is displayed in the main part.
    /// </summary>
    private object? mainView;

    /// <summary>
    ///     Gets or sets the view that is displayed in the footer part.
    /// </summary>
    [ApplicationPart(ApplicationElementPart.Footer)]
    public object? FooterView
    {
        get => this.footerView;
        set =>
            this.SetField(
                ref this.footerView,
                value);
    }

    /// <summary>
    ///     Gets or sets the view that is displayed in the header part.
    /// </summary>
    [ApplicationPart(ApplicationElementPart.Header)]
    public object? HeaderView
    {
        get => this.headerView;
        set =>
            this.SetField(
                ref this.headerView,
                value);
    }

    /// <summary>
    ///     Gets or sets the view that is displayed in the main part.
    /// </summary>
    [ApplicationPart(ApplicationElementPart.Main)]
    public object? MainView
    {
        get => this.mainView;
        set =>
            this.SetField(
                ref this.mainView,
                value);
    }
}
