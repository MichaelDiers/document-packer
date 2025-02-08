namespace DocumentPacker.Parts.Main.EncryptPart;

/// <summary>
///     A drag and drop handler for file names.
/// </summary>
public interface IHandleDragAndDrop
{
    /// <summary>
    ///     Determines whether this instance can handle drag and drop for the specified files.
    /// </summary>
    /// <param name="files">The files that are dropped.</param>
    /// <returns>
    ///     <c>true</c> if this instance can handle drag and drop for the specified files; otherwise, <c>false</c>.
    /// </returns>
    bool CanHandleDragAndDrop(IEnumerable<string> files);

    /// <summary>
    ///     Handles the drag and drop for the specified files.
    /// </summary>
    /// <param name="files">The files that are dropped.</param>
    void HandleDragAndDrop(IEnumerable<string> files);
}
