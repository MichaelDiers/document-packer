namespace DocumentPacker.Services;

using System.Security.Cryptography;

/// <inheritdoc cref="IRsaService" />
public class RsaService : IRsaService
{
    /// <summary>
    ///     The default size for new rsa keys.
    /// </summary>
    private const int KeySize = 4096;

    private readonly RSAEncryptionPadding encryptionPadding = RSAEncryptionPadding.OaepSHA512;

    /// <summary>
    ///     Generate a private and public rsa keys pair.
    /// </summary>
    /// <returns>A tuple of the generated keys.</returns>
    public (string privateKey, string publicKey) GenerateKeys()
    {
        using var rsa = RSA.Create(RsaService.KeySize);
        return (rsa.ExportRSAPrivateKeyPem(), rsa.ExportRSAPublicKeyPem());
    }

    /// <summary>
    ///     Validates the given private and public rsa keys pair.
    /// </summary>
    /// <param name="privateKey">The pem formatted private rsa key.</param>
    /// <param name="publicKey">The pem formatted public rsa key.</param>
    /// <returns><c>True</c> if the key pair is valid; <c>false</c> otherwise </returns>
    public bool ValidateKeys(string privateKey, string publicKey)
    {
        var data = new byte[16];
        RandomNumberGenerator.Create().GetNonZeroBytes(data);

        try
        {
            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKey);

            var encrypted = rsa.Encrypt(
                data,
                this.encryptionPadding);

            rsa.ImportFromPem(privateKey);
            var decrypted = rsa.Decrypt(
                encrypted,
                this.encryptionPadding);

            return data.Length == decrypted.Length &&
            Enumerable.Range(
                    0,
                    data.Length)
                .All(index => data[index] == decrypted[index]);
        }
        catch
        {
            return false;
        }
    }
}
