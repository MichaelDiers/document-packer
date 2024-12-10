namespace DocumentPacker;

using DocumentPacker.Parts.Contracts;

/// <summary>
///     The event args used when an application part is requested.
/// </summary>
/// <seealso cref="System.EventArgs" />
public class RequestViewEventArgs(Part part) : EventArgs
{
    /// <summary>
    ///     Gets the requested application part.
    /// </summary>
    public Part Part { get; } = part;
}
