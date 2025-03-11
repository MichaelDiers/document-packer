namespace DocumentPacker.Services;

using System.IO;

public interface IDecryptService
{
    Task<string> DecryptAsync(
        string privateRsaKey,
        FileInfo encryptedFile,
        DirectoryInfo outputFolder,
        CancellationToken cancellationToken
    );
}
