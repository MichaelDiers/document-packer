namespace DocumentPacker.Parts.Header.Links.ChangeLanguageLinkPart;

using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class ChangeLanguageLinkPartServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the language link part of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddLanguageLinkPart(this IServiceCollection services)
    {
        services.AddKeyedTransient<IApplicationView, ChangeLanguageLinkView>(ApplicationElementPart.ChangeLanguageLink);
        services.AddKeyedTransient<IApplicationViewModel, ChangeLanguageLinkViewModel>(
            ApplicationElementPart.ChangeLanguageLink);

        return services;
    }
}
