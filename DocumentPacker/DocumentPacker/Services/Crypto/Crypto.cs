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

    /// <inheritdoc cref="ICrypto.DecryptAsync(byte[],CancellationToken)" />
    public async Task<byte[]> DecryptAsync(byte[] data, CancellationToken cancellationToken)
    {
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
}
