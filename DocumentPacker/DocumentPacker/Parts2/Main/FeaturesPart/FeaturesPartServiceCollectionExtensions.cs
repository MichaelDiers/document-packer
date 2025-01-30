namespace DocumentPacker.Parts2.Main.FeaturesPart;

using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class FeaturesPartServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the features part of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddFeaturesPart(this IServiceCollection services)
    {
        services.AddKeyedTransient<IApplicationView, FeaturesView>(ApplicationElementPart.Features);
        services.AddKeyedTransient<IApplicationViewModel, FeaturesViewModel>(ApplicationElementPart.Features);

        return services;
    }
}
