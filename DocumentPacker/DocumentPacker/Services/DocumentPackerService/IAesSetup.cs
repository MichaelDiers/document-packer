namespace DocumentPacker.Services.DocumentPackerService;

using System.Security.Cryptography;

/// <summary>
///     Set up the aes encryption process.
/// </summary>
public interface IAesSetup
{
    /// <summary>
    ///     Set the aes key size.
    /// </summary>
    /// <param name="keySize">Size of the aes key.</param>
    /// <param name="paddingMode">The aes paddingMode mode.</param>
    /// <returns>A reference to <see cref="IAddData" />.</returns>
    IAddData SetupAes(int keySize = 256, PaddingMode paddingMode = PaddingMode.PKCS7);
}
