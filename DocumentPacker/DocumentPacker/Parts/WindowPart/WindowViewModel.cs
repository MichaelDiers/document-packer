namespace DocumentPacker.Parts.WindowPart;

using System.Windows;
using DocumentPacker.Commands;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using Libs.Wpf.Localization;

/// <summary>
///     View model of the <see cref="WindowView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class WindowViewModel : ApplicationBaseViewModel, ICancelCommandViewModel
{
    /// <summary>
    ///     A value that indicates if a command is active.
    /// </summary>
    private bool isCommandActive;

    /// <summary>
    ///     A value that indicates if the <see cref="Window.IsEnabled" />.
    /// </summary>
    private bool isWindowEnabled;

    /// <summary>
    ///     The title of the <see cref="Window" />.
    /// </summary>
    private Translatable title = new(
        WindowPartTranslation.ResourceManager,
        nameof(WindowPartTranslation.Title));

    /// <summary>
    ///     The translatable cancellable button.
    /// </summary>
    private TranslatableCancellableButton? translatableCancellableButton;

    /// <summary>
    ///     The view that is displayed in the window.
    /// </summary>
    private object? view;

    /// <summary>
    ///     Initializes a new instance of the <see cref="WindowViewModel" /> class.
    /// </summary>
    public WindowViewModel(ICommandSync commandSync)
    {
        this.CommandSync = commandSync;
    }

    /// <summary>
    ///     Gets the <see cref="ICommandSync" />.
    /// </summary>
    public ICommandSync CommandSync { get; }

    /// <summary>
    ///     Gets or sets a value that indicates if a command is active.
    /// </summary>
    public bool IsCommandActive
    {
        get => this.isCommandActive;
        set =>
            this.SetField(
                ref this.isCommandActive,
                value);
    }

    /// <summary>
    ///     Gets or sets a value that indicates if the <see cref="Window.IsEnabled" />.
    /// </summary>
    public bool IsWindowEnabled
    {
        get => this.isWindowEnabled;
        set =>
            this.SetField(
                ref this.isWindowEnabled,
                value);
    }

    /// <summary>
    ///     Gets or sets the title of the <see cref="Window" />.
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
    ///     Gets or sets the translatable cancellable button.
    /// </summary>
    public TranslatableCancellableButton? TranslatableCancellableButton
    {
        get => this.translatableCancellableButton;
        set =>
            this.SetField(
                ref this.translatableCancellableButton,
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
