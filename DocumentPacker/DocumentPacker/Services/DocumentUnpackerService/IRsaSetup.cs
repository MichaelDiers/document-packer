namespace DocumentPacker.Services.DocumentUnpackerService;

using System.Security.Cryptography;

/// <summary>
///     Set up the rsa decryption process.
/// </summary>
public interface IRsaSetup
{
    /// <summary>
    ///     Set the rsa private key.
    /// </summary>
    /// <param name="privateKeyPem">The rsa private key pem.</param>
    /// <param name="padding">Specifies the rsa encryption padding.</param>
    /// <returns>The <see cref="IAesSetup" />.</returns>
    IAesSetup SetupRsa(string privateKeyPem, RSAEncryptionPadding? padding = null);
}
