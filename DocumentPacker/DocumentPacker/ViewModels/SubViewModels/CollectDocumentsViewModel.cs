namespace DocumentPacker.ViewModels.SubViewModels;

using DocumentPacker.Contracts;
using DocumentPacker.Contracts.ViewModels.SubViewModels;

/// <summary>
///     View model for the collect documents view.
/// </summary>
/// <seealso cref="DocumentPacker.ViewModels.SubViewModels.SubViewModel" />
/// <seealso cref="DocumentPacker.Contracts.ViewModels.SubViewModels.ICollectDocumentsViewModel" />
internal class CollectDocumentsViewModel() : SubViewModel(SubViewId.CollectDocuments), ICollectDocumentsViewModel
{
}
