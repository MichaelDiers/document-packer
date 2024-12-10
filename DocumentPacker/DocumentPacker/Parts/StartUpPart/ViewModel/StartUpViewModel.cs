namespace DocumentPacker.Parts.StartUpPart.ViewModel;

using DocumentPacker.Parts.Contracts;
using DocumentPacker.Parts.StartUpPart.Contracts;
using DocumentPacker.Parts.ViewModel;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     The view model data that are displayed at application startup.
/// </summary>
/// <param name="links">The available links from the startup view.</param>
/// <seealso cref="IStartUpViewModel" />
internal class StartUpViewModel(IEnumerable<IStartUpLinkViewModel> links) : PartViewModel(Part.StartUp),
    IStartUpViewModel
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="StartUpViewModel" /> class.
    /// </summary>
    public StartUpViewModel()
        : this(App.ServiceProvider.GetServices<IStartUpLinkViewModel>())
    {
    }

    /// <summary>
    ///     Gets the links that can be followed from the startup view.
    /// </summary>
    public IEnumerable<IStartUpLinkViewModel> Links { get; } = links;
}
