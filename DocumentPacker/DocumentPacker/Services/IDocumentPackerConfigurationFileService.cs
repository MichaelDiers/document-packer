namespace DocumentPacker.Services;

using System.IO;
using System.Security;
using DocumentPacker.Models;

public interface IDocumentPackerConfigurationFileService
{
    Task<ConfigurationModel> FromFileAsync(
        FileInfo configurationFile,
        SecureString password,
        CancellationToken cancellationToken
    );

    Task ToFileAsync(
        FileInfo privateConfigurationFile,
        FileInfo publicConfigurationFile,
        SecureString password,
        ConfigurationModel configurationModel,
        CancellationToken cancellationToken
    );
}
