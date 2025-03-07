namespace DocumentPacker.Parts.WindowPart;

using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using Libs.Wpf.ViewModels;

/// <summary>
///     View model of the <see cref="WindowView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class WindowViewModel : ApplicationBaseViewModel
{
    /// <summary>
    ///     The fatal error message text.
    /// </summary>
    private string? fatalErrorMessage;

    /// <summary>
    ///     The title.
    /// </summary>
    private Translatable title = new(
        WindowPartTranslation.ResourceManager,
        nameof(WindowPartTranslation.Title));

    /// <summary>
    ///     The view that is displayed in the window.
    /// </summary>
    private object? view;

    /// <summary>
    ///     Gets or sets the fatal error message text.
    /// </summary>
    public string? FatalErrorMessage
    {
        get => this.fatalErrorMessage;
        set =>
            this.SetField(
                ref this.fatalErrorMessage,
                value);
    }

    /// <summary>
    ///     Gets or sets the title.
    /// </summary>
    public Translatable Title
    {
        get => this.title;
        set =>
            this.SetField(
                ref this.title,
                value);
    }

    /// <summary>
    ///     Gets or sets the view that is displayed in the window.
    /// </summary>
    [ApplicationPart(ApplicationElementPart.Layout)]
    public object? View
    {
        get => this.view;
        set =>
            this.SetField(
                ref this.view,
                value);
    }
}
