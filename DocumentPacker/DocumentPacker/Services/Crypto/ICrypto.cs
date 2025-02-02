namespace DocumentPacker.Services.Crypto;

using System.IO;

/// <summary>
///     Encrypt and decrypt data.
/// </summary>
public interface ICrypto
{
    /// <summary>
    ///     Decrypts the given <paramref name="data" />.
    /// </summary>
    /// <param name="data">The data to be decrypted including a header.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the decrypted <paramref name="data" />.</returns>
    Task<byte[]> DecryptAsync(byte[] data, CancellationToken cancellationToken);

    /// <summary>
    ///     Decrypts the given <paramref name="data" />.
    /// </summary>
    /// <param name="data">The data including a header to be decrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the decrypted <paramref name="data" />.</returns>
    Task<string> DecryptAsync(string data, CancellationToken cancellationToken);

    /// <summary>
    ///     Decrypts the given <paramref name="encrypted" /> to the <paramref name="decrypted" /> file.
    /// </summary>
    /// <param name="encrypted">The encrypted file.</param>
    /// <param name="decrypted">The <paramref name="encrypted" /> file is decrypted to this file.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result indicates success.</returns>
    Task DecryptAsync(FileInfo encrypted, FileInfo decrypted, CancellationToken cancellationToken);

    /// <summary>
    ///     Decrypts the given <see cref="input" /> stream and writes the result to the <see cref="output" /> stream.
    /// </summary>
    /// <param name="input">The encrypted data stream.</param>
    /// <param name="output">The decrypted output stream.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
    Task DecryptAsync(Stream input, Stream output, CancellationToken cancellationToken);

    /// <summary>
    ///     Encrypts the given <paramref name="data" />.
    /// </summary>
    /// <param name="data">The data to be encrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the encrypted <paramref name="data" /> and a header.</returns>
    Task<byte[]> EncryptAsync(byte[] data, CancellationToken cancellationToken);

    /// <summary>
    ///     Encrypts the given <paramref name="data" />.
    /// </summary>
    /// <param name="data">The data to be encrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the encrypted <paramref name="data" /> and a header.</returns>
    Task<string> EncryptAsync(string data, CancellationToken cancellationToken);

    /// <summary>
    ///     Encrypts the given <see cref="input" /> file and writes the result to the <see cref="output" /> file.
    /// </summary>
    /// <param name="input">The plain text data file.</param>
    /// <param name="output">The encrypted output file.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
    Task EncryptAsync(FileInfo input, FileInfo output, CancellationToken cancellationToken);

    /// <summary>
    ///     Encrypts the given <see cref="input" /> stream and writes the result to the <see cref="output" /> stream.
    /// </summary>
    /// <param name="input">The plain text data stream.</param>
    /// <param name="output">The encrypted output stream.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
    Task EncryptAsync(Stream input, Stream output, CancellationToken cancellationToken);
}
