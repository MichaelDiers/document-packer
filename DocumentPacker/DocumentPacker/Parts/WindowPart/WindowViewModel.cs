namespace DocumentPacker.Parts.WindowPart;

using System.Windows;
using DocumentPacker.Commands;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using Libs.Wpf.ViewModels;

/// <summary>
///     View model of the <see cref="WindowView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class WindowViewModel : ApplicationBaseViewModel, ITranslatableCancellableButton
{
    private readonly ICommandSync commandSync;

    /// <summary>
    ///     The is command active.
    /// </summary>
    private bool isCommandActive;

    /// <summary>
    ///     The is window enabled.
    /// </summary>
    private bool isWindowEnabled;

    /// <summary>
    ///     The title.
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
        this.commandSync = commandSync;

        commandSync.CommandSyncChanged += this.OnCommandSyncChanged;
        this.IsCommandActive = false;
        this.IsWindowEnabled = !this.IsCommandActive;
    }

    /// <summary>
    ///     Gets or sets the is command active.
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
    ///     Gets or sets the is window enabled.
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

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public override void Dispose()
    {
        this.commandSync.CommandSyncChanged -= this.OnCommandSyncChanged;
        base.Dispose();
    }

    private void OnCommandSyncChanged(object? sender, CommandSyncChangedEventArgs e)
    {
        if (Application.Current.Dispatcher.Thread != Thread.CurrentThread)
        {
            Application.Current.Dispatcher.Invoke(
                () => this.OnCommandSyncChanged(
                    sender,
                    e));
        }

        this.IsCommandActive = e.IsCommandActive;
        this.isWindowEnabled = !this.IsCommandActive;

        this.TranslatableCancellableButton = e is {IsCommandActive: true, TranslatableCancellableButton: not null}
            ? e.TranslatableCancellableButton
            : null;
    }
}
