namespace DocumentPacker.Services.Crypto;

using System.Security;
using System.Security.Cryptography;
using DocumentPacker.Extensions;

/// <inheritdoc cref="Crypto" />
internal class AesCrypto(SecureString secureString) : Crypto(AlgorithmIdentifier.Aes)
{
    /// <inheritdoc cref="Crypto.CreateSymmetricAlgorithm" />
    protected override SymmetricAlgorithm CreateSymmetricAlgorithm()
    {
        var aes = Aes.Create();
        aes.Key = secureString.AsByte();
        aes.Padding = PaddingMode.PKCS7;
        return aes;
    }
}
