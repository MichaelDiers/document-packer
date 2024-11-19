namespace DocumentPacker.ViewModels.SubViewModels;

using DocumentPacker.Contracts;
using DocumentPacker.Contracts.ViewModels.SubViewModels;

/// <summary>
///     View model for the load configuration view.
/// </summary>
/// <seealso cref="DocumentPacker.ViewModels.SubViewModels.SubViewModel" />
/// <seealso cref="DocumentPacker.Contracts.ViewModels.SubViewModels.ILoadConfigurationViewModel" />
internal class LoadConfigurationViewModel() : SubViewModel(SubViewId.LoadConfiguration), ILoadConfigurationViewModel
{
}
