namespace DocumentPacker.Services.Crypto;

using System.Net;
using System.Security;
using System.Security.Cryptography;

/// <inheritdoc cref="Crypto" />
internal class AesCrypto(SecureString secureString) : Crypto(AlgorithmIdentifier.Aes)
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AesCrypto" /> class.
    /// </summary>
    /// <param name="key">The aes key.</param>
    public AesCrypto(string key)
        : this(AesCrypto.ToSecureString(key))
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AesCrypto" /> class.
    /// </summary>
    /// <param name="key">The aes key.</param>
    public AesCrypto(byte[] key)
        : this(AesCrypto.ToSecureString(key))
    {
    }

    /// <inheritdoc cref="Crypto.CreateSymmetricAlgorithm" />
    protected override SymmetricAlgorithm CreateSymmetricAlgorithm()
    {
        var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(
            new NetworkCredential(
                string.Empty,
                secureString).Password);
        aes.Padding = PaddingMode.PKCS7;
        return aes;
    }

    /// <summary>
    ///     Converts to from <see cref="string" /> to <see cref="SecureString" />.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>A <see cref="SecureString" />.</returns>
    private static SecureString ToSecureString(string key)
    {
        var secureString = new SecureString();
        foreach (var character in key)
        {
            secureString.AppendChar(character);
        }

        return secureString;
    }

    /// <summary>
    ///     Converts to from <see cref="byte" /> array to <see cref="SecureString" />.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>A <see cref="SecureString" />.</returns>
    private static SecureString ToSecureString(byte[] key)
    {
        return AesCrypto.ToSecureString(Convert.ToBase64String(key));
    }
}
