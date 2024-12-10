namespace DocumentPacker.Parts.DocumentPackerPart.ViewModel;

using DocumentPacker.ApplicationInit.Configuration;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Contracts;
using DocumentPacker.Parts.DocumentPackerPart.Contracts;
using DocumentPacker.Parts.StartUpPart.Contracts;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     The main view model of the application that is the data context of the application window.
/// </summary>
/// <param name="title">The title of the application window.</param>
/// <param name="version">The version of the application.</param>
/// <param name="icon">The icon of the application window.</param>
/// <param name="viewModelPart">The view model of the application part.</param>
internal class DocumentPackerViewModel(
    string title,
    string version,
    string icon,
    IPartViewModel viewModelPart
) : BaseViewModel, IDocumentPackerViewModel
{
    /// <summary>
    ///     The icon of the application window.
    /// </summary>
    private string icon = icon;

    /// <summary>
    ///     The title of the application window.
    /// </summary>
    private string title = title;

    /// <summary>
    ///     The version of the application.
    /// </summary>
    private string version = version;

    /// <summary>
    ///     The viewModelPart.
    /// </summary>
    private IPartViewModel viewModelPart = viewModelPart;

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
            App.ServiceProvider.GetRequiredService<IStartUpViewModel>())
    {
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
}
