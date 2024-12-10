namespace DocumentPacker.Parts.Contracts;

using System.ComponentModel;

/// <summary>
///     Base view model for an application part.
/// </summary>
/// <seealso cref="INotifyPropertyChanged" />
public interface IPartViewModel : INotifyPropertyChanged
{
    /// <summary>
    ///     Gets the part specification.
    /// </summary>
    Part Part { get; }
}
