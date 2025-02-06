namespace DocumentPacker.Services.DocumentPackerService;

using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using DocumentPacker.Services.PackerStream;

/// <summary>
///     A fluent implementation to create a document packer file.
/// </summary>
/// <seealso cref="DocumentPacker.Services.DocumentPackerService.ICreateArchive" />
/// <seealso cref="DocumentPacker.Services.DocumentPackerService.IRsaSetup" />
/// <seealso cref="DocumentPacker.Services.DocumentPackerService.IAesSetup" />
/// <seealso cref="DocumentPacker.Services.DocumentPackerService.IAddData" />
internal class DocumentPackerService : ICreateArchive, IRsaSetup, IAesSetup, IAddData
{
    /// <summary>
    ///     The files that are included in the document packer file.
    /// </summary>
    private readonly List<(string entryName, FileInfo fileInfo)> files = new();

    /// <summary>
    ///     The plain texts that are included in the document packer file.
    /// </summary>
    private readonly List<(string entryName, string text)> texts = new();

    /// <summary>
    ///     The aes key size to encrypt <see cref="files" /> and <see cref="texts" />.
    /// </summary>
    private int? aesKeySize;

    /// <summary>
    ///     The aes padding mode.
    /// </summary>
    private PaddingMode aesPaddingMode;

    /// <summary>
    ///     The document packer file name.
    /// </summary>
    private string? archiveFileName;

    /// <summary>
    ///     Indicates that the processing already terminated.
    /// </summary>
    private bool closed;

    /// <summary>
    ///     The RSA encryption padding.
    /// </summary>
    private RSAEncryptionPadding? rsaEncryptionPadding;

    /// <summary>
    ///     The RSA public key pem used to encrypt the aes key.
    /// </summary>
    private string? rsaPublicKeyPem;

    /// <summary>
    ///     Adds the specified <paramref name="fileInfo" /> using the given <paramref name="entryName" />.
    /// </summary>
    /// <param name="entryName">Name of the entry.</param>
    /// <param name="fileInfo">The file information.</param>
    /// <returns>A self reference.</returns>
    public IAddData Add(string entryName, FileInfo fileInfo)
    {
        if (this.files.Any(tuple => tuple.entryName == entryName) ||
            this.texts.Any(tuple => tuple.entryName == entryName))
        {
            throw new InvalidOperationException("Entry already exists.");
        }

        this.files.Add((entryName, fileInfo));

        return this;
    }

    /// <summary>
    ///     Adds the specified <paramref name="text" /> using the given <paramref name="entryName" />.
    /// </summary>
    /// <param name="entryName">Name of the entry.</param>
    /// <param name="text">The text to add.</param>
    /// <returns>A self reference.</returns>
    public IAddData Add(string entryName, string text)
    {
        if (this.files.Any(tuple => tuple.entryName == entryName) ||
            this.texts.Any(tuple => tuple.entryName == entryName))
        {
            throw new InvalidOperationException("Entry already exists.");
        }

        this.texts.Add((entryName, text));

        return this;
    }

    /// <summary>
    ///     Create a new archive.
    /// </summary>
    /// <param name="filePath">The file path of the new archive.</param>
    /// <returns>The <see cref="IRsaSetup" />.</returns>
    public IRsaSetup CreateArchive(string filePath)
    {
        if (File.Exists(filePath))
        {
            throw new InvalidOperationException("File already exists.");
        }

        this.archiveFileName = filePath;

        return this;
    }

    /// <summary>
    ///     Start to compress, encrypt and pack the specified files and text.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (this.closed)
        {
            throw new InvalidOperationException("Already executed.");
        }

        this.closed = true;

        if (this.archiveFileName is null)
        {
            throw new InvalidOperationException("Missing archive file name.");
        }

        if (this.aesKeySize is null)
        {
            throw new InvalidOperationException("Missing aes setup.");
        }

        if (this.rsaEncryptionPadding is null)
        {
            throw new InvalidOperationException("Missing rsa encryption padding.");
        }

        using var zipArchive = ZipFile.Open(
            this.archiveFileName,
            ZipArchiveMode.Create);

        using var aes = Aes.Create();
        aes.Padding = this.aesPaddingMode;
        aes.KeySize = this.aesKeySize.Value;
        aes.GenerateKey();
        aes.GenerateIV();

        using (var rsa = RSA.Create())
        {
            rsa.ImportFromPem(this.rsaPublicKeyPem);
            var encryptedAesKey = rsa.Encrypt(
                aes.Key,
                this.rsaEncryptionPadding);
            var zipArchiveEntry = zipArchive.CreateEntry(
                nameof(Aes),
                CompressionLevel.NoCompression);
            await using var zipArchiveEntryStream = zipArchiveEntry.Open();
            await zipArchiveEntryStream.WriteAsync(
                encryptedAesKey,
                cancellationToken);
        }

        foreach (var (entryName, text) in this.texts)
        {
            var zipArchiveEntry = zipArchive.CreateEntry(
                entryName,
                CompressionLevel.NoCompression);
            await using var zipArchiveEntryStream = zipArchiveEntry.Open();
            await using var packerStream = new PackerStream(
                zipArchiveEntryStream,
                PackerStreamMode.Pack,
                aes.Key,
                aes.Padding);
            await packerStream.WriteAsync(
                Encoding.UTF8.GetBytes(text),
                cancellationToken);
        }

        foreach (var (entryName, fileInfo) in this.files)
        {
            var zipArchiveEntry = zipArchive.CreateEntry(
                entryName,
                CompressionLevel.NoCompression);
            await using var zipArchiveEntryStream = zipArchiveEntry.Open();
            await using var packerStream = new PackerStream(
                zipArchiveEntryStream,
                PackerStreamMode.Pack,
                aes.Key,
                aes.Padding);
            await using var fileStream = fileInfo.OpenRead();
            await fileStream.CopyToAsync(
                packerStream,
                cancellationToken);
        }
    }

    /// <summary>
    ///     Set the aes key size.
    /// </summary>
    /// <param name="keySize">Size of the aes key.</param>
    /// <param name="paddingMode">The aes paddingMode mode.</param>
    /// <returns>A reference to <see cref="IAddData" />.</returns>
    public IAddData SetupAes(int keySize = 256, PaddingMode paddingMode = PaddingMode.PKCS7)
    {
        if (this.aesKeySize is not null)
        {
            throw new InvalidOperationException("Aes setup already done.");
        }

        this.aesKeySize = keySize;
        this.aesPaddingMode = paddingMode;
        return this;
    }

    /// <summary>
    ///     Set the rsa public key.
    /// </summary>
    /// <param name="publicKeyPem">The rsa public key pem.</param>
    /// <param name="padding">Specifies the rsa encryption padding.</param>
    /// <returns>The <see cref="IAesSetup" />.</returns>
    public IAesSetup SetupRsa(string publicKeyPem, RSAEncryptionPadding? padding = null)
    {
        if (this.rsaPublicKeyPem is not null)
        {
            throw new InvalidOperationException("Rsa setup already done.");
        }

        this.rsaPublicKeyPem = publicKeyPem;
        this.rsaEncryptionPadding = padding ?? RSAEncryptionPadding.OaepSHA512;

        return this;
    }
}
