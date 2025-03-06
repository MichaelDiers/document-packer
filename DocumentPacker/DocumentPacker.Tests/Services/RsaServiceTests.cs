namespace DocumentPacker.Tests.Services;

using DocumentPacker.Services;
using Libs.Wpf.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

public class RsaServiceTests
{
    private readonly IRsaService service = CustomServiceProviderBuilder
        .Build(ServiceCollectionExtensionsForServices.TryAddAllServices)
        .GetRequiredService<IRsaService>();

    [Fact]
    public void GenerateKeys()
    {
        var (privateKey, publicKey) = this.service.GenerateKeys();

        Assert.False(string.IsNullOrWhiteSpace(privateKey));
        Assert.False(string.IsNullOrWhiteSpace(publicKey));
    }

    [Fact]
    public void ValidateKeys()
    {
        var (privateKey, publicKey) = this.service.GenerateKeys();

        Assert.False(string.IsNullOrWhiteSpace(privateKey));
        Assert.False(string.IsNullOrWhiteSpace(publicKey));

        Assert.True(
            this.service.ValidateKeys(
                privateKey,
                publicKey));
    }

    [Fact]
    public void ValidateKeys_ShouldFail_IfKeysAreInvalid()
    {
        Assert.False(
            this.service.ValidateKeys(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()));
    }

    [Fact]
    public void ValidateKeys_ShouldFail_IfPrivateAndPublicKeyDoNotMatch()
    {
        var (privateKey1, publicKey1) = this.service.GenerateKeys();
        var (privateKey2, publicKey2) = this.service.GenerateKeys();

        Assert.False(
            this.service.ValidateKeys(
                privateKey1,
                publicKey2));

        Assert.False(
            this.service.ValidateKeys(
                privateKey2,
                publicKey1));
    }
}
