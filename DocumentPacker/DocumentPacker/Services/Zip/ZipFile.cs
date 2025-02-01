namespace DocumentPacker.Services.Zip;

using System.IO;
using System.IO.Compression;

/// <inheritdoc cref="IZipFile" />
internal class ZipFile : IZipFile
{
    /// <summary>
    ///     All files and text are added to this <see cref="ZipArchive" />.
    /// </summary>
    private readonly ZipArchive archive;

    /// <summary>
    ///     The file path of the zip file.
    /// </summary>
    private readonly string filePath;

    /// <summary>
    ///     The temporary directory that is used for creating the temporary zip file <see cref="tempFile" />.
    /// </summary>
    private readonly DirectoryInfo tempDirectory = Directory.CreateTempSubdirectory(Guid.NewGuid().ToString());

    /// <summary>
    ///     The temporary zip file is created in this temporary directory
    /// </summary>
    private readonly FileInfo tempFile;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ZipFile" /> class.
    /// </summary>
    /// <param name="filePath">The file path of the zip.</param>
    protected ZipFile(string filePath)
    {
        this.filePath = filePath;

        this.tempFile = new FileInfo(
            Path.Combine(
                this.tempDirectory.FullName,
                Guid.NewGuid().ToString()));

        this.archive = System.IO.Compression.ZipFile.Open(
            this.tempFile.FullName,
            ZipArchiveMode.Create);
    }

    /// <inheritdoc cref="IZipFile.AddFile" />
    public void AddFile(string sourceFileName, string entryName)
    {
        this.archive.CreateEntryFromFile(
            sourceFileName,
            entryName);
    }

    /// <inheritdoc cref="IZipFile.AddTextAsync" />
    public async Task AddTextAsync(string text, string entryName, CancellationToken cancellationToken)
    {
        var zipArchiveEntry = this.archive.CreateEntry(entryName);
        await using var writer = new StreamWriter(zipArchiveEntry.Open());
        await writer.WriteAsync(text);
    }

    /// <summary>
    ///     A factory method for <see cref="IZipFile" /> used for handling zip files.
    /// </summary>
    /// <param name="filePath">The file path of the zip.</param>
    /// <returns>The created <see cref="IZipFile" /> instance.</returns>
    /// <exception cref="ArgumentException">File already exists: {filePath}</exception>
    public static IZipFile Create(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (File.Exists(filePath))
        {
            throw new ArgumentException(
                // ReSharper disable once LocalizableElement
                $"File already exists: {filePath}",
                nameof(filePath));
        }

        File.WriteAllText(
            filePath,
            string.Empty);

        return new ZipFile(filePath);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        this.archive.Dispose();

        this.tempFile.CopyTo(
            this.filePath,
            true);

        if (this.tempFile.Exists)
        {
            this.tempFile.Delete();
        }

        if (this.tempDirectory.Exists)
        {
            this.tempDirectory.Delete(true);
        }
    }
}
