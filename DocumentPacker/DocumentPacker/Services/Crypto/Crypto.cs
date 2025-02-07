namespace DocumentPacker.Services.Crypto;

using System.IO;
using System.Security.Cryptography;

/// <inheritdoc cref="ICrypto" />
internal abstract class Crypto : ICrypto
{
    /// <summary>
    ///     The index of the algorithm header
    /// </summary>
    private const int AlgorithmHeaderIndex = Crypto.ReleaseVersionHeaderIndex + 1;

    /// <summary>
    ///     The length of the header that prepend the IV header part.
    /// </summary>
    private const int CustomHeaderLength = Crypto.IvLengthHeaderIndex + 1;

    /// <summary>
    ///     The iv header starts at this index.
    /// </summary>
    private const int IvHeaderIndex = Crypto.CustomHeaderLength;

    /// <summary>
    ///     The index at which the iv header starts.
    /// </summary>
    private const int IvLengthHeaderIndex = Crypto.AlgorithmHeaderIndex + 1;

    /// <summary>
    ///     The index of the major version.
    /// </summary>
    private const int MajorVersionHeaderIndex = 0;

    /// <summary>
    ///     The index of the minor version.
    /// </summary>
    private const int MinorVersionHeaderIndex = Crypto.MajorVersionHeaderIndex + 1;

    /// <summary>
    ///     The index of the release version.
    /// </summary>
    private const int ReleaseVersionHeaderIndex = Crypto.MinorVersionHeaderIndex + 1;

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

    /// <inheritdoc cref="ICrypto.DecryptAsync(byte[],CancellationToken)" />
    public async Task<byte[]> DecryptAsync(byte[] data, CancellationToken cancellationToken)
    {
        if (this.algorithmIdentifier == AlgorithmIdentifier.Rsa)
        {
            return await this.DecryptAsymmetricAsync(
                data,
                cancellationToken);
        }

        await using var input = new MemoryStream(data);
        await using var output = new MemoryStream();
        await this.DecryptAsync(
            input,
            output,
            cancellationToken);
        return output.ToArray();
    }

    /// <inheritdoc cref="ICrypto.DecryptAsync(string,CancellationToken)" />
    public async Task<string> DecryptAsync(string data, CancellationToken cancellationToken)
    {
        await using var input = new MemoryStream(Convert.FromBase64String(data));
        await using var output = new MemoryStream();
        await this.DecryptAsync(
            input,
            output,
            cancellationToken);

        return Convert.ToBase64String(output.ToArray());
    }

    /// <inheritdoc cref="ICrypto.DecryptAsync(FileInfo,FileInfo,CancellationToken)" />
    public async Task DecryptAsync(FileInfo encrypted, FileInfo decrypted, CancellationToken cancellationToken)
    {
        if (!encrypted.Exists)
        {
            throw new ArgumentException(
                // ReSharper disable once LocalizableElement
                $"File '{encrypted}' does not exist.",
                nameof(encrypted));
        }

        if (decrypted.Exists)
        {
            throw new ArgumentException(
                // ReSharper disable once LocalizableElement
                $"File '{decrypted}' already exist.",
                nameof(decrypted));
        }

        if (this.algorithmIdentifier == AlgorithmIdentifier.Rsa)
        {
            var decryptedData = await this.DecryptAsync(
                await File.ReadAllBytesAsync(
                    encrypted.FullName,
                    cancellationToken),
                cancellationToken);
            await File.WriteAllBytesAsync(
                decrypted.FullName,
                decryptedData,
                cancellationToken);
            return;
        }

        await using var input = new FileStream(
            encrypted.FullName,
            FileMode.Open);
        await using var output = new FileStream(
            decrypted.FullName,
            FileMode.CreateNew);
        await this.DecryptAsync(
            input,
            output,
            cancellationToken);
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
            case AlgorithmIdentifier.Rsa:
                await using (var memoryStream = new MemoryStream())
                {
                    await input.CopyToAsync(
                        memoryStream,
                        cancellationToken);
                    var decrypted = await this.DecryptAsync(
                        memoryStream.ToArray(),
                        cancellationToken);
                    await output.WriteAsync(
                        decrypted,
                        cancellationToken);
                }

                break;
            case AlgorithmIdentifier.None:
            default:
                throw new ArgumentException(
                    // ReSharper disable once LocalizableElement
                    $"Undefined {nameof(AlgorithmIdentifier)}: {this.algorithmIdentifier}",
                    nameof(Crypto.algorithmIdentifier));
        }
    }

    /// <inheritdoc cref="ICrypto.EncryptAsync(byte[],CancellationToken)" />
    public async Task<byte[]> EncryptAsync(byte[] data, CancellationToken cancellationToken)
    {
        if (this.algorithmIdentifier == AlgorithmIdentifier.Rsa)
        {
            return await this.EncryptAsymmetricAsync(
                data,
                cancellationToken);
        }

        await using var input = new MemoryStream(data);
        await using var output = new MemoryStream();
        await this.EncryptAsync(
            input,
            output,
            cancellationToken);

        return output.ToArray();
    }

    /// <inheritdoc cref="ICrypto.EncryptAsync(string,CancellationToken)" />
    public async Task<string> EncryptAsync(string data, CancellationToken cancellationToken)
    {
        await using var input = new MemoryStream(Convert.FromBase64String(data));
        await using var output = new MemoryStream();
        await this.EncryptAsync(
            input,
            output,
            cancellationToken);
        return Convert.ToBase64String(output.ToArray());
    }

    /// <inheritdoc cref="ICrypto.EncryptAsync(FileInfo,FileInfo,CancellationToken)" />
    public async Task EncryptAsync(FileInfo input, FileInfo output, CancellationToken cancellationToken)
    {
        if (!input.Exists)
        {
            throw new ArgumentException(
                // ReSharper disable once LocalizableElement
                $"File '{input}' does not exist.",
                nameof(input));
        }

        if (output.Exists)
        {
            throw new ArgumentException(
                // ReSharper disable once LocalizableElement
                $"File '{output}' already exist.",
                nameof(output));
        }

        if (this.algorithmIdentifier == AlgorithmIdentifier.Rsa)
        {
            var encrypted = await this.EncryptAsync(
                await File.ReadAllBytesAsync(
                    input.FullName,
                    cancellationToken),
                cancellationToken);
            await File.WriteAllBytesAsync(
                output.FullName,
                encrypted,
                cancellationToken);
            return;
        }

        await using var inputStream = new FileStream(
            input.FullName,
            FileMode.Open);
        await using var outputStream = new FileStream(
            output.FullName,
            FileMode.CreateNew);
        await this.EncryptAsync(
            inputStream,
            outputStream,
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
            case AlgorithmIdentifier.Rsa:
                await using (var memoryStream = new MemoryStream())
                {
                    await input.CopyToAsync(
                        memoryStream,
                        cancellationToken);
                    var encrypted = await this.EncryptAsync(
                        memoryStream.ToArray(),
                        cancellationToken);
                    await output.WriteAsync(
                        encrypted,
                        cancellationToken);
                }

                break;
            case AlgorithmIdentifier.None:
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
    ///     Decrypts the given <paramref name="data" /> by using an asymmetric algorithm.
    /// </summary>
    /// <param name="data">The data to be decrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>The decrypted <paramref name="data" />.</returns>
    protected virtual Task<byte[]> DecryptAsymmetricAsync(byte[] data, CancellationToken cancellationToken)
    {
        throw new NotImplementedException(this.algorithmIdentifier.ToString());
    }

    /// <summary>
    ///     Encrypts the given <paramref name="data" /> by using an asymmetric algorithm.
    /// </summary>
    /// <param name="data">The data to be encrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>The encrypted <paramref name="data" />.</returns>
    protected virtual Task<byte[]> EncryptAsymmetricAsync(byte[] data, CancellationToken cancellationToken)
    {
        throw new NotImplementedException(this.algorithmIdentifier.ToString());
    }

    /// <summary>
    ///     Create the header of the encrypted data.
    /// </summary>
    /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
    private byte[] CreateHeader(SymmetricAlgorithm symmetricAlgorithm)
    {
        var ivLength = symmetricAlgorithm.IV.Length;
        if (ivLength > 255)
        {
            throw new InvalidOperationException("Length of IV is not supported.");
        }

        var headerLength = Crypto.GetActualHeaderLength(
            Crypto.CustomHeaderLength,
            ivLength);

        var header = new byte[headerLength];
        header[Crypto.MajorVersionHeaderIndex] = (byte) MajorVersion.Alice;
        header[Crypto.MinorVersionHeaderIndex] = (byte) MinorVersion.Alien;
        header[Crypto.ReleaseVersionHeaderIndex] = (byte) ReleaseVersion.Amy;
        header[Crypto.AlgorithmHeaderIndex] = (byte) this.algorithmIdentifier;
        header[Crypto.IvLengthHeaderIndex] = (byte) ivLength;
        symmetricAlgorithm.IV.CopyTo(
            header,
            Crypto.IvHeaderIndex);

        return header;
    }

    /// <summary>
    ///     Decrypts the given <see cref="input" /> stream and writes the result to the <see cref="output" /> stream.
    /// </summary>
    /// <param name="symmetricAlgorithm">The algorithm that is used for decrypting.</param>
    /// <param name="input">The encrypted data stream.</param>
    /// <param name="output">The decrypted output stream.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
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
    ///     Encrypts the data from <paramref name="input" /> and writes the result to the <paramref name="output" />.
    /// </summary>
    /// <param name="symmetricAlgorithm">The symmetric algorithm used for encryption.</param>
    /// <param name="input">The input data stream.</param>
    /// <param name="output">The encrypted output data stream.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
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
    ///     Gets the actual length of the header including the unused part of the header.
    /// </summary>
    /// <param name="customHeaderLength">Length of the custom header.</param>
    /// <param name="ivLength">Length of the iv header.</param>
    /// <returns>The actual length of the header.</returns>
    private static int GetActualHeaderLength(int customHeaderLength, int ivLength)
    {
        var total = customHeaderLength + ivLength;
        if (total % 8 == 0)
        {
            return total;
        }

        return total + 8 - total % 8;
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
        var customHeader = await Crypto.ReadAsync(
            input,
            Crypto.CustomHeaderLength,
            cancellationToken);

        if ((MajorVersion) customHeader[Crypto.MajorVersionHeaderIndex] != MajorVersion.Alice ||
            (MinorVersion) customHeader[Crypto.MinorVersionHeaderIndex] != MinorVersion.Alien ||
            (ReleaseVersion) customHeader[Crypto.ReleaseVersionHeaderIndex] != ReleaseVersion.Amy ||
            (AlgorithmIdentifier) customHeader[Crypto.AlgorithmHeaderIndex] != this.algorithmIdentifier)
        {
            throw new InvalidOperationException("Invalid header.");
        }

        var iv = await Crypto.ReadAsync(
            input,
            customHeader[Crypto.IvLengthHeaderIndex],
            cancellationToken);

        var headerLength = Crypto.GetActualHeaderLength(
            customHeader.Length,
            iv.Length);
        var unusedHeaderLength = headerLength - customHeader.Length - iv.Length;
        _ = await Crypto.ReadAsync(
            input,
            unusedHeaderLength,
            cancellationToken);

        symmetricAlgorithm.IV = iv;
    }

    /// <summary>
    ///     Reads <paramref name="length" /> bytes from the <paramref name="stream" />.
    /// </summary>
    /// <param name="stream">The bytes are read from this stream.</param>
    /// <param name="length">The amount of bytes that are read.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>The requested amount of bytes.</returns>
    private static async Task<byte[]> ReadAsync(Stream stream, int length, CancellationToken cancellationToken)
    {
        if (length == 0)
        {
            return [];
        }

        var data = new byte[length];
        var actualLength = await stream.ReadAsync(
            data,
            cancellationToken);
        if (actualLength != data.Length)
        {
            throw new InvalidOperationException("Invalid stream data.");
        }

        return data;
    }
}
