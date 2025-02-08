namespace DocumentPacker.Parts.Main.ChangeLanguagePart;

using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class ChangeLanguagePartServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the change language part of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddChangeLanguagePart(this IServiceCollection services)
    {
        services.AddKeyedTransient<IApplicationView, ChangeLanguageView>(ApplicationElementPart.ChangeLanguage);
        services.AddKeyedTransient<IApplicationViewModel, ChangeLanguageViewModel>(
            ApplicationElementPart.ChangeLanguage);

        return services;
    }
}
