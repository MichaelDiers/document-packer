namespace DocumentPacker.Parts.Main.EncryptPart.Service;

using DocumentPacker.Parts.Main.EncryptPart.Model;

public interface IEncryptService
{
    Task EncryptAsync(IEncryptData data, CancellationToken cancellationToken);

    Task<(string privateKey, string publicKey)> GenerateRsaKeysAsync(CancellationToken cancellationToken);
}
