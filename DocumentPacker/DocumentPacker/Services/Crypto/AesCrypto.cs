namespace DocumentPacker.Services.Crypto;

using System.Security.Cryptography;

/// <inheritdoc cref="Crypto" />
internal class AesCrypto(byte[] key) : Crypto(AlgorithmIdentifier.Aes)
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AesCrypto" /> class.
    /// </summary>
    /// <param name="key">The aes key.</param>
    public AesCrypto(string key)
        : this(Convert.FromBase64String(key))
    {
    }

    /// <inheritdoc cref="Crypto.CreateSymmetricAlgorithm" />
    protected override SymmetricAlgorithm CreateSymmetricAlgorithm()
    {
        var aes = Aes.Create();
        aes.Key = key;
        aes.Padding = PaddingMode.PKCS7;
        return aes;
    }
}
