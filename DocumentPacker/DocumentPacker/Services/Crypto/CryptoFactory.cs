namespace DocumentPacker.Services.Crypto;

/// <inheritdoc cref="ICryptoFactory" />
public class CryptoFactory : ICryptoFactory
{
    /// <inheritdoc cref="ICryptoFactory.CreateAes(byte[])" />
    public ICrypto CreateAes(byte[] key)
    {
        return new AesCrypto(key);
    }

    /// <inheritdoc cref="ICryptoFactory.CreateAes(string)" />
    public ICrypto CreateAes(string key)
    {
        return new AesCrypto(key);
    }
}
