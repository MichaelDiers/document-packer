namespace DocumentPacker.Services.Crypto;

using System.Security.Cryptography;

/// <inheritdoc cref="Crypto" />
internal class AesCrypto(byte[] key) : Crypto(AlgorithmIdentifier.Aes)
{
    /// <inheritdoc cref="Crypto.CreateSymmetricAlgorithm" />
    protected override SymmetricAlgorithm CreateSymmetricAlgorithm()
    {
        var aes = Aes.Create();
        aes.Key = key;
        aes.Padding = PaddingMode.PKCS7;
        return aes;
    }
}
