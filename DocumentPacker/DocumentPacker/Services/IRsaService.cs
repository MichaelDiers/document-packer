namespace DocumentPacker.Services;

/// <summary>
///     A service for creating and validating rsa keys.
/// </summary>
public interface IRsaService
{
    /// <summary>
    ///     Generate a private and public rsa keys pair.
    /// </summary>
    /// <returns>A tuple of the generated keys.</returns>
    (string privateKey, string publicKey) GenerateKeys();

    /// <summary>
    ///     Validates the given private and public rsa keys pair.
    /// </summary>
    /// <param name="privateKey">The pem formatted private rsa key.</param>
    /// <param name="publicKey">The pem formatted public rsa key.</param>
    /// <returns><c>True</c> if the key pair is valid; <c>false</c> otherwise </returns>
    bool ValidateKeys(string privateKey, string publicKey);
}
