namespace DocumentPacker.Services.Crypto;

using System.Security;

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

    /// <inheritdoc cref="ICryptoFactory.CreateAes(SecureString)" />
    public ICrypto CreateAes(SecureString key)
    {
        return new AesCrypto(key);
    }
}
