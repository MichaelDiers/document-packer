namespace DocumentPacker.Services.DocumentUnpackerService;

using System.Security.Cryptography;

/// <summary>
///     Set up the aes decryption process.
/// </summary>
public interface IAesSetup
{
    /// <summary>
    ///     Start to decompress, decrypt and unpack the files and text of the document packer file.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
    Task ExecuteAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Set the aes key size.
    /// </summary>
    /// <param name="paddingMode">The aes paddingMode mode.</param>
    /// <returns>A reference to <see cref="IAesSetup" />.</returns>
    IAesSetup SetupAes(PaddingMode paddingMode = PaddingMode.PKCS7);
}
