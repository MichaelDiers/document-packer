namespace DocumentPacker.Tests.ApplicationInit.Configuration;

using DocumentPacker.ApplicationInit.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Tests for <see cref="ConfigurationServiceCollectionExtensions" />.
/// </summary>
public class ConfigurationServiceCollectionExtensionsTests
{
    /// <summary>
    ///     The provider for resolving dependencies.
    /// </summary>
    private readonly ServiceProvider provider;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConfigurationServiceCollectionExtensionsTests" /> class.
    /// </summary>
    public ConfigurationServiceCollectionExtensionsTests()
    {
        var services = new ServiceCollection();
        services.AddConfiguration();
        services.AddAppConfigurationInitializer();
        this.provider = services.BuildServiceProvider();
    }

    [Fact]
    public void AddAppConfigurationInitializer()
    {
        var initializer = this.provider.GetRequiredService<IAppConfigurationInitializer>();
        var configuration = initializer.Init();

        ConfigurationServiceCollectionExtensionsTests.AssertConfiguration(configuration);
    }

    [Fact]
    public void AddConfiguration()
    {
        var configuration = this.provider.GetRequiredService<IAppConfiguration>();

        ConfigurationServiceCollectionExtensionsTests.AssertConfiguration(configuration);
    }

    private static void AssertConfiguration(IAppConfiguration configuration)
    {
        const string expectedIcon = "Icon Test";
        const string expectedTitle = "DocumentPacker Test";
        const string expectedVersion = "v0.0.1 Test";

        Assert.Equal(
            expectedIcon,
            configuration.Icon);
        Assert.Equal(
            expectedTitle,
            configuration.Title);
        Assert.Equal(
            expectedVersion,
            configuration.Version);
    }
}
