namespace DocumentPacker.Services.Zip;

/// <summary>
///     A files and text to a zip file.
/// </summary>
/// <seealso cref="IDisposable" />
public interface IZipFile : IDisposable
{
    /// <summary>
    ///     Adds the file to the zip.
    /// </summary>
    /// <param name="sourceFileName">Name of the source file.</param>
    /// <param name="entryName">Name of the entry.</param>
    void AddFile(string sourceFileName, string entryName);

    /// <summary>
    ///     Adds the text to the zip file.
    /// </summary>
    /// <param name="text">The text that is added.</param>
    /// <param name="entryName">Name of the entry.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> that indicates success.</returns>
    Task AddTextAsync(string text, string entryName, CancellationToken cancellationToken);
}
