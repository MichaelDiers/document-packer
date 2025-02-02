namespace DocumentPacker.Services.Crypto;

using System.Security;
using DocumentPacker.Extensions;

/// <inheritdoc cref="ICryptoFactory" />
public class CryptoFactory : ICryptoFactory
{
    /// <inheritdoc cref="ICryptoFactory.CreateAes(byte[])" />
    public ICrypto CreateAes(byte[] key)
    {
        return new AesCrypto(key.AsSecureString());
    }

    /// <inheritdoc cref="ICryptoFactory.CreateAes(string)" />
    public ICrypto CreateAes(string key)
    {
        return new AesCrypto(key.AsSecureString());
    }

    /// <inheritdoc cref="ICryptoFactory.CreateAes(SecureString)" />
    public ICrypto CreateAes(SecureString key)
    {
        return new AesCrypto(key);
    }

    /// <inheritdoc cref="ICryptoFactory.CreateRsa(string?,string?)" />
    public ICrypto CreateRsa(string? privateKey, string? publicKey)
    {
        return this.CreateRsa(
            privateKey?.AsSecureString(),
            publicKey?.AsSecureString());
    }

    /// <inheritdoc cref="ICryptoFactory.CreateRsa(byte[],byte[])" />
    public ICrypto CreateRsa(byte[]? privateKey, byte[]? publicKey)
    {
        return this.CreateRsa(
            privateKey?.AsSecureString(),
            publicKey?.AsSecureString());
    }

    /// <inheritdoc cref="ICryptoFactory.CreateRsa(SecureString?,SecureString?)" />
    public ICrypto CreateRsa(SecureString? privateKey, SecureString? publicKey)
    {
        return new RsaCrypto(
            privateKey,
            publicKey);
    }
}
