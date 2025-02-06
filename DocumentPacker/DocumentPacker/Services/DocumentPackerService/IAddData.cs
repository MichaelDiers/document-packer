namespace DocumentPacker.Services.DocumentPackerService;

using System.IO;

/// <summary>
///     Add files and text to the document pack or start the execution process.
/// </summary>
public interface IAddData
{
    /// <summary>
    ///     Adds the specified <paramref name="fileInfo" /> using the given <paramref name="entryName" />.
    /// </summary>
    /// <param name="entryName">Name of the entry.</param>
    /// <param name="fileInfo">The file information.</param>
    /// <returns>A self reference.</returns>
    IAddData Add(string entryName, FileInfo fileInfo);

    /// <summary>
    ///     Adds the specified <paramref name="text" /> using the given <paramref name="entryName" />.
    /// </summary>
    /// <param name="entryName">Name of the entry.</param>
    /// <param name="text">The text to add.</param>
    /// <returns>A self reference.</returns>
    IAddData Add(string entryName, string text);

    /// <summary>
    ///     Start to compress, encrypt and pack the specified files and text.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
    Task ExecuteAsync(CancellationToken cancellationToken);
}
