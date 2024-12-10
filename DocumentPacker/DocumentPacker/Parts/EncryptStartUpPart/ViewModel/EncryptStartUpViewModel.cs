namespace DocumentPacker.Parts.EncryptStartUpPart.ViewModel;

using DocumentPacker.Parts.Contracts;
using DocumentPacker.Parts.EncryptStartUpPart.Contracts;
using DocumentPacker.Parts.ViewModel;

/// <summary>
///     The encrypt startup view model.
/// </summary>
/// <seealso cref="DocumentPacker.Parts.ViewModel.PartViewModel" />
/// <seealso cref="DocumentPacker.Parts.EncryptStartUpPart.Contracts.IEncryptStartUpViewModel" />
internal class EncryptStartUpViewModel() : PartViewModel(Part.EncryptStartUp), IEncryptStartUpViewModel
{
}
