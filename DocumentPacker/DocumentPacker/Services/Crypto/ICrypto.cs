namespace DocumentPacker.Services.Crypto;

/// <summary>
///     Encrypt and decrypt data.
/// </summary>
public interface ICrypto
{
    /// <summary>
    ///     Decrypts the given <paramref name="data" />.
    /// </summary>
    /// <param name="header">The header information of the encrypted data.</param>
    /// <param name="data">The data to be decrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the decrypted <paramref name="data" />.</returns>
    Task<byte[]> DecryptAsync(byte[] header, byte[] data, CancellationToken cancellationToken);

    /// <summary>
    ///     Encrypts the given <paramref name="data" />.
    /// </summary>
    /// <param name="data">The data to be encrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the encrypted <paramref name="data" /> and a header.</returns>
    Task<(byte[] header, byte[] data)> EncryptAsync(byte[] data, CancellationToken cancellationToken);
}
