namespace DocumentPacker.Services.Crypto;

using System.Security;
using System.Security.Cryptography;
using DocumentPacker.Extensions;

/// <inheritdoc cref="Crypto" />
internal class AesCrypto(SecureString secureString) : Crypto(AlgorithmIdentifier.Aes)
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AesCrypto" /> class.
    /// </summary>
    /// <param name="key">The aes key.</param>
    public AesCrypto(string key)
        : this(key.ToSecureString())
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AesCrypto" /> class.
    /// </summary>
    /// <param name="key">The aes key.</param>
    public AesCrypto(byte[] key)
        : this(Convert.ToBase64String(key).ToSecureString())
    {
    }

    /// <inheritdoc cref="Crypto.CreateSymmetricAlgorithm" />
    protected override SymmetricAlgorithm CreateSymmetricAlgorithm()
    {
        var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(secureString.ToUnsecureString());
        aes.Padding = PaddingMode.PKCS7;
        return aes;
    }
}
