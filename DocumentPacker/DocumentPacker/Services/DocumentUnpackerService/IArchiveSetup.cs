namespace DocumentPacker.Services.DocumentUnpackerService;

using System.IO;

/// <summary>
///     Specify the document unpacker archive information.
/// </summary>
public interface IArchiveSetup
{
    /// <summary>
    ///     Unpack the specified archive to the given destination directory.
    /// </summary>
    /// <param name="archive">The archive to unpack.</param>
    /// <param name="destination">The destination directory of the unpacked data.</param>
    /// <returns>The <see cref="IRsaSetup" />.</returns>
    IRsaSetup SetupArchive(FileInfo archive, DirectoryInfo destination);
}
