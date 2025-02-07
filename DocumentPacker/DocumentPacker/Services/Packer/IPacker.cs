namespace DocumentPacker.Services.Packer;

using System.IO;

/// <summary>
///     The packer compresses and encrypts data.
/// </summary>
public interface IPacker : IDisposable
{
    /// <summary>
    ///     Adds a file to the package.
    /// </summary>
    /// <param name="entryName">The name of the entry.</param>
    /// <param name="fileInfo">The file information of the file to add.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> that indicates success.</returns>
    Task AddAsync(string entryName, FileInfo fileInfo, CancellationToken cancellationToken);

    /// <summary>
    ///     Adds a plain text to the package.
    /// </summary>
    /// <param name="entryName">The name of the entry.</param>
    /// <param name="text">The text to add.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> that indicates success.</returns>
    Task AddAsync(string entryName, string text, CancellationToken cancellationToken);
}
