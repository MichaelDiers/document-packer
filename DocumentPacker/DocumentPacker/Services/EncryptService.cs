namespace DocumentPacker.Services;

using System.IO;
using DocumentPacker.Models;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
using DocumentPacker.Services.DocumentPackerService;

public class EncryptService(IDocumentPackerService documentPackerService) : IEncryptService
{
    public async Task EncryptAsync(string rsaPublicKey, EncryptModel encryptModel, CancellationToken cancellationToken)
    {
        var path = Path.Combine(
            encryptModel.OutputFolder,
            encryptModel.OutputFile);

        var addData = documentPackerService.CreateArchive(path).SetupRsa(rsaPublicKey).SetupAes();
        foreach (var encryptModelItem in encryptModel.Items)
        {
            if (string.IsNullOrWhiteSpace(encryptModelItem.Value))
            {
                if (encryptModelItem.IsRequired)
                {
                    throw new NotImplementedException();
                }

                continue;
            }

            switch (encryptModelItem.ConfigurationItemType)
            {
                case ConfigurationItemType.File:
                    addData = addData.Add(
                        encryptModelItem.Id,
                        new FileInfo(encryptModelItem.Value));
                    break;
                case ConfigurationItemType.Text:
                    addData = addData.Add(
                        encryptModelItem.Id,
                        encryptModelItem.Value);
                    break;
                default:
                    throw new ArgumentException($"Unsupported value: {encryptModelItem.ConfigurationItemType}");
            }
        }

        await addData.ExecuteAsync(cancellationToken);
    }
}
