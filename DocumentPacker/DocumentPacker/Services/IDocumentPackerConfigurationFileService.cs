namespace DocumentPacker.Services;

using System.IO;
using DocumentPacker.Models;

public interface IDocumentPackerConfigurationFileService
{
    Task<ConfigurationModel> FromFileAsync(
        FileInfo configurationFile,
        string password,
        CancellationToken cancellationToken
    );

    Task ToFileAsync(
        FileInfo privateConfigurationFile,
        FileInfo publicConfigurationFile,
        string password,
        ConfigurationModel configurationModel,
        CancellationToken cancellationToken
    );
}
