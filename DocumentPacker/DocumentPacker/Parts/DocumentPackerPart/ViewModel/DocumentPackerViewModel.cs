namespace DocumentPacker.Parts.DocumentPackerPart.ViewModel;

using DocumentPacker.ApplicationInit.Configuration;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Contracts;
using DocumentPacker.Parts.DocumentPackerPart.Contracts;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     The main view model of the application that is the data context of the application window.
/// </summary>
internal class DocumentPackerViewModel : BaseViewModel, IDocumentPackerViewModel
{
    /// <summary>
    ///     The icon of the application window.
    /// </summary>
    private string icon;

    /// <summary>
    ///     The title of the application window.
    /// </summary>
    private string title;

    /// <summary>
    ///     The version of the application.
    /// </summary>
    private string version;

    /// <summary>
    ///     The viewModelPart.
    /// </summary>
    private IPartViewModel viewModelPart;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DocumentPackerViewModel" /> class.
    /// </summary>
    public DocumentPackerViewModel()
        : this(App.ServiceProvider.GetRequiredService<IAppConfiguration>())
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DocumentPackerViewModel" /> class.
    /// </summary>
    /// <param name="configuration">The configuration of the application.</param>
    public DocumentPackerViewModel(IAppConfiguration configuration)
        : this(
            configuration.Title,
            configuration.Version,
            configuration.Icon,
            App.ServiceProvider.GetRequiredKeyedService<IPartViewModel>(Part.StartUp))
    {
    }

    /// <summary>
    ///     The main view model of the application that is the data context of the application window.
    /// </summary>
    /// <param name="title">The title of the application window.</param>
    /// <param name="version">The version of the application.</param>
    /// <param name="icon">The icon of the application window.</param>
    /// <param name="viewModelPart">The view model of the application part.</param>
    public DocumentPackerViewModel(
        string title,
        string version,
        string icon,
        IPartViewModel viewModelPart
    )
    {
        this.icon = icon;
        this.title = title;
        this.version = version;
        this.viewModelPart = viewModelPart;

        App.RequestViewEvent += this.HandleRequestViewEvent;
    }

    /// <summary>
    ///     Gets the icon of the application window.
    /// </summary>
    public string Icon
    {
        get => this.icon;
        set =>
            this.SetField(
                ref this.icon,
                value);
    }

    /// <summary>
    ///     Gets the title of the application window.
    /// </summary>
    public string Title
    {
        get => this.title;
        set =>
            this.SetField(
                ref this.title,
                value);
    }

    /// <summary>
    ///     Gets the version of the application.
    /// </summary>
    public string Version
    {
        get => this.version;
        set =>
            this.SetField(
                ref this.version,
                value);
    }

    /// <summary>
    ///     Gets or sets the view model part.
    /// </summary>
    public IPartViewModel ViewModelPart
    {
        get => this.viewModelPart;
        set =>
            this.SetField(
                ref this.viewModelPart,
                value);
    }

    /// <summary>
    ///     Handles the request view event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RequestViewEventArgs" /> instance containing the event data.</param>
    private void HandleRequestViewEvent(object? sender, RequestViewEventArgs e)
    {
        this.ViewModelPart = App.ServiceProvider.GetRequiredKeyedService<IPartViewModel>(e.Part);
    }
}
