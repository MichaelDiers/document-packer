namespace DocumentPacker.Parts.WindowPart;

using System.Windows;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;

/// <summary>
///     View model of the <see cref="WindowView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class WindowViewModel : ApplicationBaseViewModel
{
    /// <summary>
    ///     A value that indicates if the <see cref="Window.IsEnabled" />.
    /// </summary>
    private bool isWindowEnabled = true;

    /// <summary>
    ///     The title of the <see cref="Window" />.
    /// </summary>
    private Translatable title = new(
        WindowPartTranslation.ResourceManager,
        nameof(WindowPartTranslation.Title));

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
        this.CommandSync.CommandSyncChanged += this.OnCommandSyncChanged;
    }

    /// <summary>
    ///     Gets the <see cref="ICommandSync" />.
    /// </summary>
    public ICommandSync CommandSync { get; }

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

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public override void Dispose()
    {
        this.CommandSync.CommandSyncChanged -= this.OnCommandSyncChanged;
        base.Dispose();
    }

    /// <summary>
    ///     Handles the <see cref="ICommandSync.CommandSyncChanged" /> event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The event args.</param>
    private void OnCommandSyncChanged(object? sender, CommandSyncChangedEventArgs e)
    {
        this.IsWindowEnabled = !e.IsCommandActive;
    }
}
