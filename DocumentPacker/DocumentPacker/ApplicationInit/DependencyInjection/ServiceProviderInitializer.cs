namespace DocumentPacker.ApplicationInit.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Initializer for a <see cref="IServiceProvider" />.
/// </summary>
public static class ServiceProviderInitializer
{
    /// <summary>
    ///     Initializes an <see cref="IServiceProvider" />.
    /// </summary>
    /// <returns></returns>
    public static IServiceProvider Init()
    {
        var services = new ServiceCollection();
        services.AddDependencies();
        return services.BuildServiceProvider();
    }
}
