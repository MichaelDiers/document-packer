namespace DocumentPacker.Services.DocumentPackerService;

using System.Security.Cryptography;

/// <summary>
///     Set up the rsa encryption process.
/// </summary>
public interface IRsaSetup
{
    /// <summary>
    ///     Set the rsa public key.
    /// </summary>
    /// <param name="publicKeyPem">The rsa public key pem.</param>
    /// <param name="padding">Specifies the rsa encryption padding.</param>
    /// <returns>The <see cref="IAesSetup" />.</returns>
    IAesSetup SetupRsa(string publicKeyPem, RSAEncryptionPadding? padding = null);
}
