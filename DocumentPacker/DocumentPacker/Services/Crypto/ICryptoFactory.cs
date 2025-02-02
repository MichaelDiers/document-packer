namespace DocumentPacker.Services.Crypto;

using System.Security;

/// <summary>
///     Creates a crypto algorithm instance.
/// </summary>
public interface ICryptoFactory
{
    /// <summary>
    ///     Creates an aes instance.
    /// </summary>
    /// <param name="key">The aes key.</param>
    /// <returns>A <see cref="ICrypto" /> that implements the aes operations.</returns>
    ICrypto CreateAes(byte[] key);

    /// <summary>
    ///     Creates an aes instance.
    /// </summary>
    /// <param name="key">The aes key.</param>
    /// <returns>A <see cref="ICrypto" /> that implements the aes operations.</returns>
    ICrypto CreateAes(string key);

    /// <summary>
    ///     Creates an aes instance.
    /// </summary>
    /// <param name="key">The aes key.</param>
    /// <returns>A <see cref="ICrypto" /> that implements the aes operations.</returns>
    ICrypto CreateAes(SecureString key);
}
