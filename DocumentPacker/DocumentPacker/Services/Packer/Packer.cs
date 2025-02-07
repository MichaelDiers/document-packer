namespace DocumentPacker.Services.Packer;

using System.IO;
using System.IO.Compression;
using System.Security;
using System.Security.Cryptography;
using DocumentPacker.Extensions;
using DocumentPacker.Services.Crypto;
using DocumentPacker.Services.Zip;

/// <inheritdoc cref="IPacker" />
internal class Packer() : IPacker, IPackerFactory
{
    private const string AesEntry = nameof(Aes);
    private readonly ICrypto? crypto;

    private readonly IZipFile? zipFile;

    /// <summary>
    ///     Indicates if the package is open or closed. If the value is <c>true</c> no data can be added to the package.
    /// </summary>
    private bool isClosed;

    private Packer(IZipFile zipFile, ICrypto crypto)
        : this()
    {
        this.zipFile = zipFile;
        this.crypto = crypto;
    }

    /// <inheritdoc cref="IPacker.AddAsync(string,FileInfo,CancellationToken)" />
    public async Task AddAsync(string entryName, FileInfo fileInfo, CancellationToken cancellationToken)
    {
        if (this.isClosed || this.zipFile is null || this.crypto is null)
        {
            throw new InvalidOperationException();
        }

        var tempFile = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid().ToString()}.temp");

        try
        {
            await using var fileStream = fileInfo.OpenRead();
            await using var brotliStream = new BrotliStream(
                fileStream,
                CompressionLevel.Fastest);
            await using var encryptedStream = new FileStream(
                tempFile,
                FileMode.CreateNew);
            await this.crypto.EncryptAsync(
                brotliStream,
                encryptedStream,
                cancellationToken);
        }
        catch
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
                throw;
            }
        }

        try
        {
            this.zipFile.AddFile(
                fileInfo.FullName,
                entryName);
            File.Delete(tempFile);
        }
        catch
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
                throw;
            }
        }
    }

    /// <inheritdoc cref="IPacker.AddAsync(string,string,CancellationToken)" />
    public async Task AddAsync(string entryName, string text, CancellationToken cancellationToken)
    {
        if (this.isClosed || this.zipFile is null || this.crypto is null)
        {
            throw new InvalidOperationException();
        }

        using var inputMemoryStream = new MemoryStream(text.AsByte());
        using var outputMemoryStream = new MemoryStream();
        await using var outputBrotliStream = new BrotliStream(
            outputMemoryStream,
            CompressionLevel.Fastest);
        await inputMemoryStream.CopyToAsync(
            outputBrotliStream,
            cancellationToken);
        await outputBrotliStream.FlushAsync(cancellationToken);
        var compressed = outputMemoryStream.ToArray();

        var encrypted = await this.crypto.EncryptAsync(
            compressed,
            cancellationToken);

        await this.zipFile.AddTextAsync(
            encrypted.AsString(),
            entryName,
            cancellationToken);
    }

    /// <inheritdoc cref="IPackerFactory.CreatePackerAsync" />
    public async Task<IPacker> CreatePackerAsync(
        IZipFileCreator zipFileCreator,
        ICryptoFactory cryptoFactory,
        FileInfo fileInfo,
        SecureString rsaPublicKey,
        CancellationToken cancellationToken
    )
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();
        var aesKey = aes.Key;

        var zip = zipFileCreator.Create(fileInfo.FullName);
        var cryptoAes = cryptoFactory.CreateAes(aesKey);
        var cryptoRsa = cryptoFactory.CreateRsa(
            null,
            rsaPublicKey);

        // encrypt and save aes key
        await zip.AddTextAsync(
            (await cryptoRsa.EncryptAsync(
                aesKey,
                cancellationToken)).AsString(),
            Packer.AesEntry,
            cancellationToken);

        return new Packer(
            zip,
            cryptoAes);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        this.isClosed = true;
        this.zipFile?.Dispose();
    }
}
