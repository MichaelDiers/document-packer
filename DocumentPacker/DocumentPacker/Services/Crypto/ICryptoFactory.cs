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

    /// <summary>
    ///     Creates a rsa instance.
    /// </summary>
    /// <param name="privateKey">The rsa private key.</param>
    /// ///
    /// <param name="publicKey">The rsa public key.</param>
    /// <returns>A <see cref="ICrypto" /> that implements the rsa operations.</returns>
    ICrypto CreateRsa(string? privateKey, string? publicKey);

    /// <summary>
    ///     Creates a rsa instance.
    /// </summary>
    /// <param name="privateKey">The rsa private key.</param>
    /// ///
    /// <param name="publicKey">The rsa public key.</param>
    /// <returns>A <see cref="ICrypto" /> that implements the rsa operations.</returns>
    ICrypto CreateRsa(byte[]? privateKey, byte[]? publicKey);

    /// <summary>
    ///     Creates a rsa instance.
    /// </summary>
    /// <param name="privateKey">The rsa private key.</param>
    /// ///
    /// <param name="publicKey">The rsa public key.</param>
    /// <returns>A <see cref="ICrypto" /> that implements the rsa operations.</returns>
    ICrypto CreateRsa(SecureString? privateKey, SecureString? publicKey);
}
