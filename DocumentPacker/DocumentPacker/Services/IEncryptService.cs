namespace DocumentPacker.Services;

using DocumentPacker.Models;

public interface IEncryptService
{
    Task EncryptAsync(string rsaPublicKey, EncryptModel encryptModel, CancellationToken cancellationToken);
}
