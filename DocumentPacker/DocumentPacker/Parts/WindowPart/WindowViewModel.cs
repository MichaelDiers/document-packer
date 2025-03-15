namespace DocumentPacker.Parts.WindowPart;

using System.Windows;
using System.Windows.Input;
using DocumentPacker.Commands;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using Libs.Wpf.ViewModels;

/// <summary>
///     View model of the <see cref="WindowView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class WindowViewModel : ApplicationBaseViewModel
{
    private readonly ICommandSync commandSync;

    private readonly Stack<Cursor> cursors = new();

    /// <summary>
    ///     The fatal error message text.
    /// </summary>
    private string? fatalErrorMessage;

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
    }

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
    ///     Gets or sets the is command active.
    /// </summary>
    public bool IsCommandActive
    {
        get => this.isCommandActive;
        set
        {
            this.SetField(
                ref this.isCommandActive,
                value);
            this.IsWindowEnabled = !value;
        }
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
        this.IsCommandActive = e.IsCommandActive;
        if (e.IsCommandActive)
        {
            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    this.cursors.Push(Mouse.OverrideCursor);
                    Mouse.OverrideCursor = Cursors.Wait;
                });
        }
        else
        {
            Application.Current.Dispatcher.Invoke(
                () => { Mouse.OverrideCursor = this.cursors.TryPop(out var cursor) ? cursor : null; });
        }
    }
}
