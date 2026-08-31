using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Zoho.HttpClients.Registrars;
using Soenneker.Zoho.OpenApiClientUtil.Abstract;

namespace Soenneker.Zoho.OpenApiClientUtil.Registrars;

/// <summary>
/// Registers the configured Zoho OpenAPI client provider.
/// </summary>
public static class ZohoOpenApiClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IZohoOpenApiClientUtil"/> as a singleton service.
    /// </summary>
    public static IServiceCollection AddZohoOpenApiClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddZohoOpenApiHttpClientAsSingleton()
                .TryAddSingleton<IZohoOpenApiClientUtil, ZohoOpenApiClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IZohoOpenApiClientUtil"/> as a scoped service while retaining the singleton HTTP transport.
    /// </summary>
    public static IServiceCollection AddZohoOpenApiClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddZohoOpenApiHttpClientAsSingleton()
                .TryAddScoped<IZohoOpenApiClientUtil, ZohoOpenApiClientUtil>();

        return services;
    }
}
