namespace DocumentPacker.Parts2.Main.EncryptPart.Service;

using DocumentPacker.Parts2.Main.EncryptPart.Model;

public interface IEncryptService
{
    Task EncryptAsync(IEncryptData data, CancellationToken cancellationToken);

    Task<(string privateKey, string publicKey)> GenerateRsaKeysAsync(CancellationToken cancellationToken);
}
