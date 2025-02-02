﻿namespace DocumentPacker.Services.Crypto;

using System.IO;

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
    ///     Decrypts the given <paramref name="data" />.
    /// </summary>
    /// <param name="header">The header information of the encrypted data.</param>
    /// <param name="data">The data to be decrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the decrypted <paramref name="data" />.</returns>
    Task<string> DecryptAsync(string header, string data, CancellationToken cancellationToken);

    /// <summary>
    ///     Decrypts the given <paramref name="encrypted" /> to the <paramref name="decrypted" /> file.
    /// </summary>
    /// <param name="encrypted">The encrypted file.</param>
    /// <param name="decrypted">The <paramref name="encrypted" /> file is decrypted to this file.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result indicates success.</returns>
    Task DecryptAsync(FileInfo encrypted, FileInfo decrypted, CancellationToken cancellationToken);

    /// <summary>
    ///     Encrypts the given <paramref name="data" />.
    /// </summary>
    /// <param name="data">The data to be encrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the encrypted <paramref name="data" /> and a header.</returns>
    Task<(byte[] header, byte[] data)> EncryptAsync(byte[] data, CancellationToken cancellationToken);

    /// <summary>
    ///     Encrypts the given <paramref name="data" />.
    /// </summary>
    /// <param name="data">The data to be encrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the encrypted <paramref name="data" /> and a header.</returns>
    Task<(string header, string data)> EncryptAsync(string data, CancellationToken cancellationToken);

    /// <summary>
    ///     Encrypts the given <see cref="input" /> file and writes the result to the <see cref="output" /> file.
    /// </summary>
    /// <param name="input">The plain text data file.</param>
    /// <param name="output">The encrypted output file.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
    Task EncryptAsync(FileInfo input, FileInfo output, CancellationToken cancellationToken);
}
