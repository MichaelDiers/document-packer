namespace DocumentPacker.Services;

using System.IO;
using DocumentPacker.Services.DocumentUnpackerService;

public class DecryptService(IDocumentUnpackerService documentUnpackerService) : IDecryptService
{
    public async Task<string> DecryptAsync(
        string privateRsaKey,
        FileInfo encryptedFile,
        DirectoryInfo outputFolder,
        CancellationToken cancellationToken
    )
    {
        await documentUnpackerService.SetupArchive(
                encryptedFile,
                outputFolder)
            .SetupRsa(privateRsaKey)
            .SetupAes()
            .ExecuteAsync(cancellationToken);
        return "goo";
    }
}
