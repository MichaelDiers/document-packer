namespace DocumentPacker.Tests.ApplicationInit.DependencyInjection;

using DocumentPacker.ApplicationInit.Configuration;
using DocumentPacker.ApplicationInit.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Tests for <see cref="ServiceProviderInitializer" />.
/// </summary>
public class ServiceProviderInitializerTests
{
    [Fact]
    public void Init()
    {
        var provider = ServiceProviderInitializer.Init();

        var configuration = provider.GetRequiredService<IAppConfiguration>();

        Assert.NotNull(configuration);
    }
}
