using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Zoho.HttpClients.Registrars;
using Soenneker.Zoho.OpenApiClientUtil.Abstract;

namespace Soenneker.Zoho.OpenApiClientUtil.Registrars;

/// <summary>
/// Registers the OpenAPI client utility for dependency injection.
/// </summary>
public static class ZohoOpenApiClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="ZohoOpenApiClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddZohoOpenApiClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddZohoOpenApiHttpClientAsSingleton()
                .TryAddSingleton<IZohoOpenApiClientUtil, ZohoOpenApiClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="ZohoOpenApiClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddZohoOpenApiClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddZohoOpenApiHttpClientAsSingleton()
                .TryAddScoped<IZohoOpenApiClientUtil, ZohoOpenApiClientUtil>();

        return services;
    }
}
