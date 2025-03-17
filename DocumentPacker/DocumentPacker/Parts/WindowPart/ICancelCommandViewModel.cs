namespace DocumentPacker.Parts.WindowPart;

using System.ComponentModel;
using System.Windows;
using DocumentPacker.Commands;
using Libs.Wpf.ViewModels;

/// <summary>
///     The required data of the <see cref="CancelCommandBehavior" />.
/// </summary>
public interface ICancelCommandViewModel : INotifyPropertyChanged
{
    /// <summary>
    ///     Gets the <see cref="ICommandSync" />.
    /// </summary>
    public ICommandSync CommandSync { get; }

    /// <summary>
    ///     Gets or sets a value that indicates if a command is active.
    /// </summary>
    public bool IsCommandActive { get; set; }

    /// <summary>
    ///     Gets or sets a value that indicates if the <see cref="UIElement.IsEnabled" />.
    /// </summary>
    public bool IsWindowEnabled { get; set; }

    /// <summary>
    ///     Gets or sets the data of a cancellable command.
    /// </summary>
    public TranslatableCancellableButton? TranslatableCancellableButton { get; set; }
}
