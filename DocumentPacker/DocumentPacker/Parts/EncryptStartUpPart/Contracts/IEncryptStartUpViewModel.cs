namespace DocumentPacker.Parts.EncryptStartUpPart.Contracts;

/// <summary>
///     The encrypt startup view model.
/// </summary>
internal interface IEncryptStartUpViewModel
{
    /// <summary>
    ///     Gets the links of the view.
    /// </summary>
    IEnumerable<IEncryptStartUpLinkViewModel> Links { get; }
}
