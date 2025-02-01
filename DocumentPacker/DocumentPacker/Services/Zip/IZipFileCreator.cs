namespace DocumentPacker.Services.Zip;

/// <summary>
///     A factory for <see cref="IZipFile" />.
/// </summary>
public interface IZipFileCreator
{
    /// <summary>
    ///     Creates a new <see cref="ZipFile" /> at the specified file path.
    /// </summary>
    /// <param name="filePath">The file path of the new zip file.</param>
    /// <returns>A <see cref="ZipFile" />.</returns>
    IZipFile Create(string filePath);
}
