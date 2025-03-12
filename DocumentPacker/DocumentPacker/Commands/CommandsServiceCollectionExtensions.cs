namespace DocumentPacker.Commands;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />.
/// </summary>
public static class CommandsServiceCollectionExtensions
{
    /// <summary>
    ///     Add dependencies within the namespace <see cref="DocumentPacker.Commands" />.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddCommands(this IServiceCollection services)
    {
        services.TryAddSingleton<ICommandSync, CommandSync>();

        return services;
    }
}
