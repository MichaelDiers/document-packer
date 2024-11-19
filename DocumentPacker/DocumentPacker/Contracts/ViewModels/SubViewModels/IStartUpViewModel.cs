namespace DocumentPacker.Contracts.ViewModels.SubViewModels;

using System.Windows.Input;

/// <summary>
///     View model for the startup view.
/// </summary>
/// <seealso cref="DocumentPacker.Contracts.ViewModels.SubViewModels.ISubViewModel" />
public interface IStartUpViewModel : ISubViewModel
{
    /// <summary>
    ///     Gets the select next view command.
    /// </summary>
    ICommand SelectNextViewCommand { get; }
}
