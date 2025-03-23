namespace DocumentPacker.Tests.Services;

using DocumentPacker.Models;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
using DocumentPacker.Services;
using Libs.Wpf.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

public class DocumentPackerConfigurationFileServiceTests : IDisposable
{
    private readonly ConfigurationModel configurationModel = new(
        [
            new ConfigurationItemModel(
                ConfigurationItemType.File,
                true,
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()),
            new ConfigurationItemModel(
                ConfigurationItemType.Text,
                false,
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString())
        ],
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString());

    private readonly string password = Guid.NewGuid().ToString();

    private readonly FileInfo privateConfigurationFile = new(Guid.NewGuid().ToString());

    private readonly FileInfo publicConfigurationFile = new(Guid.NewGuid().ToString());

    private readonly IDocumentPackerConfigurationFileService service = CustomServiceProviderBuilder
        .Build(ServicesServiceCollectionExtensions.TryAddServices)
        .GetRequiredService<IDocumentPackerConfigurationFileService>();

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        if (this.privateConfigurationFile.Exists)
        {
            this.privateConfigurationFile.Delete();
        }

        if (this.publicConfigurationFile.Exists)
        {
            this.publicConfigurationFile.Delete();
        }
    }

    [Fact]
    public async Task FromFileAsync()
    {
        await this.service.ToFileAsync(
            this.privateConfigurationFile,
            this.publicConfigurationFile,
            this.password,
            this.configurationModel,
            TestContext.Current.CancellationToken);

        Assert.True(this.privateConfigurationFile.Exists);
        Assert.True(this.publicConfigurationFile.Exists);

        var privateConfiguration = await this.service.FromFileAsync(
            this.privateConfigurationFile,
            this.password,
            TestContext.Current.CancellationToken);
        this.AssertConfiguration(privateConfiguration);

        var publicConfiguration = await this.service.FromFileAsync(
            this.publicConfigurationFile,
            this.password,
            TestContext.Current.CancellationToken);
        Assert.Null(publicConfiguration.RsaPrivateKey);
        publicConfiguration.RsaPrivateKey = privateConfiguration.RsaPrivateKey;
        this.AssertConfiguration(publicConfiguration);
    }

    [Fact]
    public async Task ToFileAsync()
    {
        await this.service.ToFileAsync(
            this.privateConfigurationFile,
            this.publicConfigurationFile,
            this.password,
            this.configurationModel,
            TestContext.Current.CancellationToken);

        Assert.True(this.privateConfigurationFile.Exists);
        Assert.True(this.publicConfigurationFile.Exists);
    }

    private void AssertConfiguration(ConfigurationModel actual)
    {
        Assert.Equal(
            this.configurationModel.Description,
            actual.Description);
        Assert.Equal(
            this.configurationModel.RsaPrivateKey,
            actual.RsaPrivateKey);
        Assert.Equal(
            this.configurationModel.RsaPublicKey,
            actual.RsaPublicKey);
        Assert.Equal(
            this.configurationModel.ConfigurationItems.Count(),
            actual.ConfigurationItems.Count());
        foreach (var expectedItem in this.configurationModel.ConfigurationItems)
        {
            Assert.Contains(
                actual.ConfigurationItems,
                item => expectedItem.IsRequired == item.IsRequired &&
                        expectedItem.ItemDescription == item.ItemDescription &&
                        expectedItem.ConfigurationItemType == item.ConfigurationItemType);
        }
    }
}
