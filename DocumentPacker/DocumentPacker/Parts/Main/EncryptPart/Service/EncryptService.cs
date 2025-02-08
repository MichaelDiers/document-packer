namespace DocumentPacker.Parts.Main.EncryptPart.Service;

using System.Security.Cryptography;
using DocumentPacker.Parts.Main.EncryptPart.Model;

internal class EncryptService : IEncryptService
{
    public async Task EncryptAsync(IEncryptData data, CancellationToken cancellationToken)
    {
        await Task.Delay(
            3000,
            cancellationToken);
    }

    public async Task<(string privateKey, string publicKey)> GenerateRsaKeysAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        using var rsa = RSA.Create(4096);
        return (rsa.ExportRSAPrivateKeyPem(), rsa.ExportRSAPublicKeyPem());
    }
}
