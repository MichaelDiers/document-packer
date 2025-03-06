namespace DocumentPacker.Services;

using System.IO;
using System.Text;
using System.Text.Json;
using DocumentPacker.Models;
using DocumentPacker.Services.PackerStream;

internal class DocumentPackerConfigurationFileService : IDocumentPackerConfigurationFileService
{
    public async Task<ConfigurationModel> FromFileAsync(
        FileInfo configurationFile,
        string password,
        CancellationToken cancellationToken
    )
    {
        var aesKey = this.ToAesPassword(password);
        return await this.FromFileAsync(
            configurationFile,
            aesKey,
            cancellationToken);
    }

    public async Task ToFileAsync(
        FileInfo privateConfigurationFile,
        FileInfo publicConfigurationFile,
        string password,
        ConfigurationModel configurationModel,
        CancellationToken cancellationToken
    )
    {
        var aesKey = this.ToAesPassword(password);
        await this.ToFileAsync(
            privateConfigurationFile,
            aesKey,
            configurationModel,
            cancellationToken);

        var privateRsaKey = configurationModel.RsaPrivateKey;
        configurationModel.RsaPrivateKey = null;
        await this.ToFileAsync(
            publicConfigurationFile,
            aesKey,
            configurationModel,
            cancellationToken);
        configurationModel.RsaPrivateKey = privateRsaKey;
    }

    private async Task<ConfigurationModel> FromFileAsync(
        FileInfo configurationFile,
        byte[] aesKey,
        CancellationToken cancellationToken
    )
    {
        try
        {
            await using var memoryStream = new MemoryStream();

            await using var fileStream = configurationFile.OpenRead();
            await using (var packerStream = new PackerStream.PackerStream(
                             fileStream,
                             PackerStreamMode.Unpack,
                             aesKey))
            {
                await packerStream.CopyToAsync(
                    memoryStream,
                    cancellationToken);
            }

            var byteString = memoryStream.ToArray();
            var jsonString = Encoding.UTF8.GetString(byteString);
            var configuration = JsonSerializer.Deserialize<ConfigurationModel>(jsonString);

            return configuration ??
                   throw new InvalidOperationException("Cannot deserialize document packer configuration file.");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "Cannot unpack document packer configuration.",
                ex);
        }
    }

    private byte[] ToAesPassword(string password)
    {
        var bytePassword = Encoding.UTF8.GetBytes(password);
        var sizes = new[]
        {
            256 / 8,
            192 / 8,
            128 / 8,
            0
        };

        if (bytePassword.Length >= sizes[0])
        {
            return bytePassword[..sizes[0]];
        }

        for (var i = 1; i < sizes.Length; i++)
        {
            if (bytePassword.Length > sizes[i])
            {
                return bytePassword.Concat(new byte[sizes[i - 1] - bytePassword.Length]).ToArray();
            }
        }

        throw new InvalidOperationException();
    }

    private async Task ToFileAsync(
        FileInfo configurationFile,
        byte[] aesKey,
        ConfigurationModel configurationModel,
        CancellationToken cancellationToken
    )
    {
        var jsonString = JsonSerializer.Serialize(configurationModel);
        var byteString = Encoding.UTF8.GetBytes(jsonString);
        await using var privateConfigurationFileStream = configurationFile.OpenWrite();
        await using var packerStream = new PackerStream.PackerStream(
            privateConfigurationFileStream,
            PackerStreamMode.Pack,
            aesKey);
        await packerStream.WriteAsync(
            byteString,
            cancellationToken);
    }
}
