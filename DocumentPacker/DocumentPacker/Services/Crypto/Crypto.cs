namespace DocumentPacker.Services.Crypto;

using System.IO;
using System.Security.Cryptography;

/// <inheritdoc cref="ICrypto" />
internal abstract class Crypto : ICrypto
{
    /// <summary>
    ///     The algorithm identifier that is added to the header.
    /// </summary>
    private readonly AlgorithmIdentifier algorithmIdentifier;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Crypto" /> class.
    /// </summary>
    /// <param name="algorithmIdentifier">The algorithm identifier.</param>
    /// <exception cref="ArgumentException">
    ///     Unspecified {nameof(AlgorithmIdentifier)}: {algorithmIdentifier} -
    ///     algorithmIdentifier
    /// </exception>
    protected Crypto(AlgorithmIdentifier algorithmIdentifier)
    {
        if (algorithmIdentifier == AlgorithmIdentifier.None)
        {
            throw new ArgumentException(
                // ReSharper disable once LocalizableElement
                $"Unspecified {nameof(AlgorithmIdentifier)}: {algorithmIdentifier}",
                nameof(algorithmIdentifier));
        }

        this.algorithmIdentifier = algorithmIdentifier;
    }

    /// <inheritdoc cref="ICrypto.DecryptAsync(byte[],byte[],CancellationToken)" />
    public async Task<byte[]> DecryptAsync(byte[] header, byte[] data, CancellationToken cancellationToken)
    {
        if (this.algorithmIdentifier == AlgorithmIdentifier.Aes)
        {
            return await this.DecryptSymmetricAlgorithmAsync(
                header,
                data,
                cancellationToken);
        }

        throw new NotSupportedException(this.algorithmIdentifier.ToString());
    }

    /// <inheritdoc cref="ICrypto.DecryptAsync(string,string,CancellationToken)" />
    public async Task<string> DecryptAsync(string header, string data, CancellationToken cancellationToken)
    {
        var decrypted = await this.DecryptAsync(
            Convert.FromBase64String(header),
            Convert.FromBase64String(data),
            cancellationToken);

        return Convert.ToBase64String(decrypted);
    }

    /// <inheritdoc cref="ICrypto.DecryptAsync(FileInfo,FileInfo,CancellationToken)" />
    public async Task DecryptAsync(FileInfo encrypted, FileInfo decrypted, CancellationToken cancellationToken)
    {
        if (!encrypted.Exists)
        {
            throw new ArgumentException(
                $"File '{encrypted}' does not exist.",
                nameof(encrypted));
        }

        if (decrypted.Exists)
        {
            throw new ArgumentException(
                $"File '{decrypted}' already exist.",
                nameof(decrypted));
        }

        var encryptedBytes = await File.ReadAllBytesAsync(
            encrypted.FullName,
            cancellationToken);

        if (this.algorithmIdentifier == AlgorithmIdentifier.Aes)
        {
            using var aes = this.CreateSymmetricAlgorithm();
            var headerLength = aes.IV.Length + 1;
            if (headerLength % 8 != 0)
            {
                headerLength++;
            }

            var header = encryptedBytes[..headerLength];
            var data = encryptedBytes[headerLength..];

            var decryptedBytes = await this.DecryptAsync(
                header,
                data,
                cancellationToken);
            await File.WriteAllBytesAsync(
                decrypted.FullName,
                decryptedBytes,
                cancellationToken);
        }
    }

    /// <inheritdoc cref="ICrypto.DecryptAsync(Stream,Stream,CancellationToken)" />
    public async Task DecryptAsync(Stream input, Stream output, CancellationToken cancellationToken)
    {
        switch (this.algorithmIdentifier)
        {
            case AlgorithmIdentifier.Aes:
                await this.DecryptAsync(
                    this.CreateSymmetricAlgorithm(),
                    input,
                    output,
                    cancellationToken);
                break;
            default:
                throw new ArgumentException(
                    // ReSharper disable once LocalizableElement
                    $"Undefined {nameof(AlgorithmIdentifier)}: {this.algorithmIdentifier}",
                    nameof(Crypto.algorithmIdentifier));
        }
    }

    /// <inheritdoc cref="ICrypto.EncryptAsync(byte[],CancellationToken)" />
    public async Task<(byte[] header, byte[] data)> EncryptAsync(byte[] data, CancellationToken cancellationToken)
    {
        if (this.algorithmIdentifier == AlgorithmIdentifier.Aes)
        {
            return await this.EncryptSymmetricAlgorithmAsync(
                data,
                cancellationToken);
        }

        throw new NotSupportedException(this.algorithmIdentifier.ToString());
    }

    /// <inheritdoc cref="ICrypto.EncryptAsync(string,CancellationToken)" />
    public async Task<(string header, string data)> EncryptAsync(string data, CancellationToken cancellationToken)
    {
        var (header, encrypted) = await this.EncryptAsync(
            Convert.FromBase64String(data),
            cancellationToken);

        return (Convert.ToBase64String(header), Convert.ToBase64String(encrypted));
    }

    /// <inheritdoc cref="ICrypto.EncryptAsync(FileInfo,FileInfo,CancellationToken)" />
    public async Task EncryptAsync(FileInfo input, FileInfo output, CancellationToken cancellationToken)
    {
        if (!input.Exists)
        {
            throw new ArgumentException(
                $"File '{input}' does not exist.",
                nameof(input));
        }

        if (output.Exists)
        {
            throw new ArgumentException(
                $"File '{output}' already exist.",
                nameof(output));
        }

        var (header, data) = await this.EncryptAsync(
            await File.ReadAllBytesAsync(
                input.FullName,
                cancellationToken),
            cancellationToken);

        await using var stream = new FileStream(
            output.FullName,
            FileMode.CreateNew);
        await stream.WriteAsync(
            header,
            0,
            header.Length,
            cancellationToken);
        await stream.WriteAsync(
            data,
            0,
            data.Length,
            cancellationToken);
    }

    /// <inheritdoc cref="ICrypto.EncryptAsync(Stream,Stream,CancellationToken)" />
    public async Task EncryptAsync(Stream input, Stream output, CancellationToken cancellationToken)
    {
        switch (this.algorithmIdentifier)
        {
            case AlgorithmIdentifier.Aes:
                await this.EncryptAsync(
                    this.CreateSymmetricAlgorithm(),
                    input,
                    output,
                    cancellationToken);
                break;
            default:
                throw new ArgumentException(
                    // ReSharper disable once LocalizableElement
                    $"Undefined {nameof(AlgorithmIdentifier)}: {this.algorithmIdentifier}",
                    nameof(Crypto.algorithmIdentifier));
        }
    }

    /// <summary>
    ///     Creates the symmetric algorithm.
    /// </summary>
    /// <returns>The created <see cref="SymmetricAlgorithm" />.</returns>
    protected virtual SymmetricAlgorithm CreateSymmetricAlgorithm()
    {
        throw new NotSupportedException(this.algorithmIdentifier.ToString());
    }

    /// <summary>
    ///     Create the header of the encrypted data.
    /// </summary>
    /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
    private byte[] CreateHeader(SymmetricAlgorithm symmetricAlgorithm)
    {
        var headerLength = symmetricAlgorithm.IV.Length + 1;
        if (headerLength % 8 != 0)
        {
            headerLength++;
        }

        var header = new byte[headerLength];
        header[0] = (byte) this.algorithmIdentifier;
        symmetricAlgorithm.IV.CopyTo(
            header,
            1);

        return header;
    }

    private async Task DecryptAsync(
        SymmetricAlgorithm symmetricAlgorithm,
        Stream input,
        Stream output,
        CancellationToken cancellationToken
    )
    {
        await this.ProcessHeaderAsync(
            symmetricAlgorithm,
            input,
            cancellationToken);

        var cryptoTransform = symmetricAlgorithm.CreateDecryptor(
            symmetricAlgorithm.Key,
            symmetricAlgorithm.IV);

        await using var cryptoStream = new CryptoStream(
            output,
            cryptoTransform,
            CryptoStreamMode.Write);

        await input.CopyToAsync(
            cryptoStream,
            cancellationToken);

        await cryptoStream.FlushFinalBlockAsync(cancellationToken);
    }

    /// <summary>
    ///     Decrypts the given <paramref name="data" />.
    /// </summary>
    /// <param name="header">The header information of the encrypted data.</param>
    /// <param name="data">The data to be decrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the decrypted <paramref name="data" />.</returns>
    private async Task<byte[]> DecryptSymmetricAlgorithmAsync(
        byte[] header,
        byte[] data,
        CancellationToken cancellationToken
    )
    {
        using var symmetricAlgorithm = this.CreateSymmetricAlgorithm();

        this.ProcessHeader(
            symmetricAlgorithm,
            header);

        var cryptoTransform = symmetricAlgorithm.CreateDecryptor(
            symmetricAlgorithm.Key,
            symmetricAlgorithm.IV);

        return await this.RunAsync(
            data,
            cryptoTransform,
            cancellationToken);
    }

    private async Task EncryptAsync(
        SymmetricAlgorithm symmetricAlgorithm,
        Stream input,
        Stream output,
        CancellationToken cancellationToken
    )
    {
        var header = this.CreateHeader(symmetricAlgorithm);
        await output.WriteAsync(
            header,
            cancellationToken);

        var cryptoTransform = symmetricAlgorithm.CreateEncryptor(
            symmetricAlgorithm.Key,
            symmetricAlgorithm.IV);

        await using var cryptoStream = new CryptoStream(
            output,
            cryptoTransform,
            CryptoStreamMode.Write);

        await input.CopyToAsync(
            cryptoStream,
            cancellationToken);

        await cryptoStream.FlushFinalBlockAsync(cancellationToken);
    }

    /// <summary>
    ///     Encrypts the given <paramref name="data" />.
    /// </summary>
    /// <param name="data">The data to be encrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the encrypted <paramref name="data" /> and a header.</returns>
    private async Task<(byte[] header, byte[] data)> EncryptSymmetricAlgorithmAsync(
        byte[] data,
        CancellationToken cancellationToken
    )
    {
        using var symmetricAlgorithm = this.CreateSymmetricAlgorithm();

        var header = this.CreateHeader(symmetricAlgorithm);

        var cryptoTransform = symmetricAlgorithm.CreateEncryptor(
            symmetricAlgorithm.Key,
            symmetricAlgorithm.IV);

        var encrypted = await this.RunAsync(
            data,
            cryptoTransform,
            cancellationToken);

        return (header, encrypted);
    }

    /// <summary>
    ///     Reads the header of the encrypted data.
    /// </summary>
    /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
    /// <param name="header">The header information of the encrypted data.</param>
    private void ProcessHeader(SymmetricAlgorithm symmetricAlgorithm, byte[] header)
    {
        var headerLength = symmetricAlgorithm.IV.Length + 1;
        if (headerLength % 8 != 0)
        {
            headerLength++;
        }

        if (headerLength != header.Length)
        {
            throw new InvalidOperationException("Invalid header.");
        }

        var actualAlgorithmIdentifier = (AlgorithmIdentifier) header[0];
        if (actualAlgorithmIdentifier != this.algorithmIdentifier)
        {
            throw new InvalidOperationException("Invalid header.");
        }

        var iv = new byte[symmetricAlgorithm.IV.Length];
        header[1..(iv.Length + 1)]
        .CopyTo(
            iv,
            0);
        symmetricAlgorithm.IV = iv;
    }

    /// <summary>
    ///     Reads the header of the encrypted data.
    /// </summary>
    /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
    /// <param name="input">The input data stream.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    private async Task ProcessHeaderAsync(
        SymmetricAlgorithm symmetricAlgorithm,
        Stream input,
        CancellationToken cancellationToken
    )
    {
        var headerLength = symmetricAlgorithm.IV.Length + 1;
        if (headerLength % 8 != 0)
        {
            headerLength++;
        }

        var header = new byte[headerLength];
        var actualHeaderLength = await input.ReadAsync(
            header,
            cancellationToken);

        if (headerLength != actualHeaderLength)
        {
            throw new InvalidOperationException("Invalid header.");
        }

        var actualAlgorithmIdentifier = (AlgorithmIdentifier) header[0];
        if (actualAlgorithmIdentifier != this.algorithmIdentifier)
        {
            throw new InvalidOperationException("Invalid header.");
        }

        var iv = new byte[symmetricAlgorithm.IV.Length];
        header[1..(iv.Length + 1)]
        .CopyTo(
            iv,
            0);
        symmetricAlgorithm.IV = iv;
    }

    /// <summary>
    ///     Encrypts or decrypts the <paramref name="data" /> depending on the given <paramref name="cryptoTransform" />.
    /// </summary>
    /// <param name="data">The data to process.</param>
    /// <param name="cryptoTransform">The crypto transform for en- or decrypting.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>The processed data.</returns>
    private async Task<byte[]> RunAsync(
        byte[] data,
        ICryptoTransform cryptoTransform,
        CancellationToken cancellationToken
    )
    {
        await using var memoryStream = new MemoryStream();
        await using var cryptoStream = new CryptoStream(
            memoryStream,
            cryptoTransform,
            CryptoStreamMode.Write);
        await cryptoStream.WriteAsync(
            data,
            cancellationToken);
        await cryptoStream.FlushFinalBlockAsync(cancellationToken);
        return memoryStream.ToArray();
    }
}
