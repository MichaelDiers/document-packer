namespace DocumentPacker.Services.Crypto;

using System.Security;
using System.Security.Cryptography;
using DocumentPacker.Extensions;

/// <inheritdoc cref="Crypto" />
internal class RsaCrypto(
    SecureString? privateKey,
    SecureString? publicKey,
    RSAEncryptionPadding? rsaEncryptionPadding = null
) : Crypto(AlgorithmIdentifier.Rsa)
{
    /// <inheritdoc cref="Crypto.DecryptAsymmetricAsync(byte[],CancellationToken)" />
    protected override Task<byte[]> DecryptAsymmetricAsync(byte[] data, CancellationToken cancellationToken)
    {
        if (privateKey is null)
        {
            throw new InvalidOperationException("Missing private key");
        }

        var rsa = RSA.Create();

        rsa.ImportFromPem(privateKey.AsString());

        return Task.FromResult(
            rsa.Decrypt(
                data,
                rsaEncryptionPadding ?? RSAEncryptionPadding.OaepSHA512));
    }

    /// <inheritdoc cref="Crypto.EncryptAsymmetricAsync(byte[],CancellationToken)" />
    protected override Task<byte[]> EncryptAsymmetricAsync(byte[] data, CancellationToken cancellationToken)
    {
        if (publicKey is null)
        {
            throw new InvalidOperationException("Missing public key");
        }

        var rsa = RSA.Create();
        rsa.ImportFromPem(publicKey.AsString());

        return Task.FromResult(
            rsa.Encrypt(
                data,
                rsaEncryptionPadding ?? RSAEncryptionPadding.OaepSHA512));
    }
}
