namespace DocumentPacker.Services.DocumentUnpackerService;

using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using DocumentPacker.Services.PackerStream;

/// <summary>
///     A fluent implementation to unpack a document packer file.
/// </summary>
/// <seealso cref="DocumentPacker.Services.DocumentUnpackerService.IArchiveSetup" />
/// <seealso cref="DocumentPacker.Services.DocumentUnpackerService.IRsaSetup" />
/// <seealso cref="DocumentPacker.Services.DocumentUnpackerService.IAesSetup" />
internal class DocumentUnpackerService : IRsaSetup, IAesSetup, IDocumentUnpackerService
{
    /// <summary>
    ///     The aes padding mode.
    /// </summary>
    private PaddingMode? aesPaddingMode;

    /// <summary>
    ///     The document packer file.
    /// </summary>
    private FileInfo? archiveFile;

    /// <summary>
    ///     The RSA padding.
    /// </summary>
    private RSAEncryptionPadding? rsaPadding;

    /// <summary>
    ///     The RSA private key pem.
    /// </summary>
    private string? rsaPrivateKeyPem;

    /// <summary>
    ///     The destination directory for unpacked files.
    /// </summary>
    private DirectoryInfo? unpackDestination;

    /// <summary>
    ///     Start to decompress, decrypt and unpack the files and text of the document packer file.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var zipArchive = this.OpenReadZipFile();

        var aesKey = await this.ReadAesKeyAsync(
            zipArchive,
            nameof(Aes),
            cancellationToken);

        using var aes = this.InitializeAes(aesKey);

        var destination = this.HandleUnpackDestination();

        await DocumentUnpackerService.Unpack(
            zipArchive,
            aes,
            destination,
            cancellationToken);
    }

    /// <summary>
    ///     Set the aes key size.
    /// </summary>
    /// <param name="paddingMode">The aes paddingMode mode.</param>
    /// <returns>A reference to <see cref="IAesSetup" />.</returns>
    public IAesSetup SetupAes(PaddingMode paddingMode = PaddingMode.PKCS7)
    {
        DocumentUnpackerService.SetIfNull(
            ref this.aesPaddingMode,
            paddingMode,
            nameof(paddingMode));

        return this;
    }

    /// <summary>
    ///     Unpack the specified archive to the given destination directory.
    /// </summary>
    /// <param name="archive">The archive to unpack.</param>
    /// <param name="destination">The destination directory of the unpacked data.</param>
    /// <returns>The <see cref="IRsaSetup" />.</returns>
    public IRsaSetup SetupArchive(FileInfo archive, DirectoryInfo destination)
    {
        if (!archive.Exists)
        {
            throw new InvalidOperationException($"File does not exist: {archive.FullName}");
        }

        if (destination.Exists && (destination.EnumerateFiles().Any() || destination.EnumerateDirectories().Any()))
        {
            throw new InvalidOperationException($"Destination directory is not empty: {destination.FullName}");
        }

        DocumentUnpackerService.SetIfNull(
            ref this.archiveFile,
            archive,
            nameof(archive));
        DocumentUnpackerService.SetIfNull(
            ref this.unpackDestination,
            destination,
            nameof(destination));

        return this;
    }

    /// <summary>
    ///     Set the rsa private key.
    /// </summary>
    /// <param name="privateKeyPem">The rsa private key pem.</param>
    /// <param name="padding">Specifies the rsa encryption padding.</param>
    /// <returns>The <see cref="IAesSetup" />.</returns>
    public IAesSetup SetupRsa(string privateKeyPem, RSAEncryptionPadding? padding = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(privateKeyPem);

        DocumentUnpackerService.SetIfNull(
            ref this.rsaPrivateKeyPem,
            privateKeyPem,
            nameof(privateKeyPem));
        DocumentUnpackerService.SetIfNull(
            ref this.rsaPadding,
            padding ?? RSAEncryptionPadding.OaepSHA512,
            nameof(padding));

        return this;
    }

    /// <summary>
    ///     Checks the <see cref="unpackDestination" /> and creates it if necessary.
    /// </summary>
    /// <returns>The path to the unpack destination.</returns>
    /// <exception cref="InvalidOperationException">Missing value: unpack destination</exception>
    private string HandleUnpackDestination()
    {
        if (this.unpackDestination is null)
        {
            throw new InvalidOperationException("Missing value: unpack destination");
        }

        if (!this.unpackDestination.Exists)
        {
            this.unpackDestination.Create();
        }

        return this.unpackDestination.FullName;
    }

    /// <summary>
    ///     Initializes the aes.
    /// </summary>
    /// <param name="aesKey">The aes key.</param>
    /// <returns>A new <see cref="Aes" /> instance.</returns>
    /// <exception cref="InvalidOperationException">Missing value: aes padding mode</exception>
    private Aes InitializeAes(byte[] aesKey)
    {
        if (this.aesPaddingMode is null)
        {
            throw new InvalidOperationException("Missing value: aes padding mode");
        }

        var aes = Aes.Create();
        aes.Key = aesKey;
        aes.Padding = this.aesPaddingMode.Value;

        return aes;
    }

    /// <summary>
    ///     Checks if <see cref="archiveFile" /> is set and opens the <see cref="ZipArchive" /> in read only mode.
    /// </summary>
    /// <returns>The opened <see cref="ZipArchive" />.</returns>
    /// <exception cref="InvalidOperationException">Missing value: archive file</exception>
    private ZipArchive OpenReadZipFile()
    {
        if (this.archiveFile is null)
        {
            throw new InvalidOperationException("Missing value: archive file");
        }

        return ZipFile.OpenRead(this.archiveFile.FullName);
    }

    /// <summary>
    ///     Reads the aes key from the given <paramref name="zipArchive" />.
    /// </summary>
    /// <param name="zipArchive">The zip archive that contains the aes key entry.</param>
    /// <param name="aesEntryName">The name of the aes zip archive entry.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>The decrypted aes key.</returns>
    /// <exception cref="InvalidOperationException">Invalid zip archive.</exception>
    private async Task<byte[]> ReadAesKeyAsync(
        ZipArchive zipArchive,
        string aesEntryName,
        CancellationToken cancellationToken
    )
    {
        if (this.rsaPadding is null)
        {
            throw new InvalidOperationException("Missing value: rsa padding");
        }

        if (string.IsNullOrWhiteSpace(this.rsaPrivateKeyPem))
        {
            throw new InvalidOperationException("Missing value: rsa private key");
        }

        using var rsa = RSA.Create();
        rsa.ImportFromPem(this.rsaPrivateKeyPem);
        var zipArchiveEntry = zipArchive.GetEntry(aesEntryName);
        if (zipArchiveEntry is null)
        {
            throw new InvalidOperationException("Invalid zip archive.");
        }

        await using var zipArchiveEntryStream = zipArchiveEntry.Open();
        using var memoryStream = new MemoryStream();
        await zipArchiveEntryStream.CopyToAsync(
            memoryStream,
            cancellationToken);
        var encryptedAesKey = memoryStream.ToArray();

        return rsa.Decrypt(
            encryptedAesKey,
            this.rsaPadding);
    }

    /// <summary>
    ///     Sets <paramref name="current" /> to <paramref name="newValue" /> if <paramref name="current" /> is <c>null</c>;
    ///     otherwise throw an <see cref="InvalidOperationException" />.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <param name="current">A reference to the current value that is expected to be <c>null</c>.</param>
    /// <param name="newValue">The new value of <paramref name="current" />.</param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <exception cref="InvalidOperationException">Throw if <paramref name="current" /> is not <c>null</c>.</exception>
    private static void SetIfNull<T>(ref T? current, T? newValue, string parameterName)
    {
        if (current is not null)
        {
            throw new InvalidOperationException($"Value already set: {parameterName}");
        }

        current = newValue;
    }

    /// <summary>
    ///     Unpacks the specified zip archive.
    /// </summary>
    /// <param name="zipArchive">The zip archive including aes encrypted data.</param>
    /// <param name="aes">The required aes information to decrypt data.</param>
    /// <param name="destination">The destination directory for unpacked data.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    private static async Task Unpack(
        ZipArchive zipArchive,
        Aes aes,
        string destination,
        CancellationToken cancellationToken
    )
    {
        foreach (var zipArchiveEntry in zipArchive.Entries.Where(entry => entry.Name != nameof(Aes)))
        {
            await using var packerStream = new PackerStream(
                zipArchiveEntry.Open(),
                PackerStreamMode.Unpack,
                aes.Key,
                aes.Padding);
            await using var fileStream = new FileStream(
                Path.Combine(
                    destination,
                    zipArchiveEntry.Name),
                FileMode.CreateNew);
            await packerStream.CopyToAsync(
                fileStream,
                cancellationToken);
        }
    }
}
