namespace DocumentPacker.Parts.ViewModel;

using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Contracts;

/// <summary>
///     Base view model for an application part.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.BaseViewModel" />
/// <seealso cref="DocumentPacker.Parts.Contracts.IPartViewModel" />
internal class PartViewModel(Part part) : BaseViewModel, IPartViewModel
{
    /// <summary>
    ///     Gets the part specification.
    /// </summary>
    public Part Part { get; } = part;
}
