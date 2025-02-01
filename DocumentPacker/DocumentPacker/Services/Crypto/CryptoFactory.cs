namespace DocumentPacker.Services.Crypto;

/// <inheritdoc cref="ICryptoFactory" />
public class CryptoFactory : ICryptoFactory
{
    /// <inheritdoc cref="ICryptoFactory.CreateAes" />
    public ICrypto CreateAes(byte[] key)
    {
        return new AesCrypto(key);
    }
}
