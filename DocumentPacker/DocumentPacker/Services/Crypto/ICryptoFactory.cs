namespace DocumentPacker.Services.Crypto;

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
}
