namespace DocumentPacker.Services.DocumentUnpackerService;

using System.Security.Cryptography;

/// <summary>
///     Set up the aes decryption process.
/// </summary>
public interface IAesSetup
{
    /// <summary>
    ///     Set the aes key size.
    /// </summary>
    /// <param name="paddingMode">The aes paddingMode mode.</param>
    /// <returns>A reference to <see cref="IAesSetup" />.</returns>
    IExecute SetupAes(PaddingMode paddingMode = PaddingMode.PKCS7);
}
