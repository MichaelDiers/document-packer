namespace DocumentPacker.Parts.EncryptStartUpPart.ViewModel;

using DocumentPacker.Parts.Contracts;
using DocumentPacker.Parts.EncryptStartUpPart.Contracts;
using DocumentPacker.Parts.ViewModel;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     The encrypt startup view model.
/// </summary>
/// <param name="links">The links of the view.</param>
/// <seealso cref="DocumentPacker.Parts.ViewModel.PartViewModel" />
internal class EncryptStartUpViewModel(IEnumerable<IEncryptStartUpLinkViewModel> links)
    : PartViewModel(Part.EncryptStartUp), IEncryptStartUpViewModel
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EncryptStartUpViewModel" /> class.
    /// </summary>
    public EncryptStartUpViewModel()
        : this(App.ServiceProvider.GetServices<IEncryptStartUpLinkViewModel>())
    {
    }

    /// <summary>
    ///     Gets the links of the view.
    /// </summary>
    public IEnumerable<IEncryptStartUpLinkViewModel> Links { get; } = links;
}
