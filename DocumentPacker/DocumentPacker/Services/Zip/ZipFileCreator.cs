namespace DocumentPacker.Services.Zip;

/// <inheritdoc cref="IZipFileCreator" />
public class ZipFileCreator : IZipFileCreator
{
    /// <summary>
    ///     Creates a new <see cref="ZipFile" /> at the specified file path.
    /// </summary>
    /// <param name="filePath">The file path of the new zip file.</param>
    /// <returns>A <see cref="ZipFile" />.</returns>
    public IZipFile Create(string filePath)
    {
        return ZipFile.Create(filePath);
    }
}
