namespace DocumentPacker.Services.DocumentPackerService;

/// <summary>
///     Create a new document packer archive.
/// </summary>
public interface ICreateArchive
{
    /// <summary>
    ///     Create a new archive.
    /// </summary>
    /// <param name="filePath">The file path of the new archive.</param>
    /// <returns>The <see cref="IRsaSetup" />.</returns>
    IRsaSetup CreateArchive(string filePath);
}
