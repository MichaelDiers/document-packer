namespace DocumentPacker.ViewModels.SubViewModels;

using DocumentPacker.Contracts;
using DocumentPacker.Contracts.ViewModels.SubViewModels;

/// <summary>
///     Describes a sub view model.
/// </summary>
/// <seealso cref="DocumentPacker.ViewModels.BaseViewModel" />
/// <seealso cref="DocumentPacker.Contracts.ViewModels.SubViewModels.ISubViewModel" />
internal class SubViewModel : BaseViewModel, ISubViewModel
{
    /// <summary>
    ///     The sub view identifier.
    /// </summary>
    private SubViewId subViewId;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SubViewModel" /> class.
    /// </summary>
    /// <param name="subViewId">The sub view identifier.</param>
    protected SubViewModel(SubViewId subViewId)
    {
        this.SubViewId = subViewId;
    }

    /// <summary>
    ///     Gets or sets the sub view identifier.
    /// </summary>
    public SubViewId SubViewId
    {
        get => this.subViewId;
        set =>
            this.SetField(
                ref this.subViewId,
                value);
    }
}
