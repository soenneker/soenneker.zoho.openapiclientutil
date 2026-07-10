using Soenneker.Zoho.OpenApiClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Zoho.OpenApiClientUtil.Abstract;

/// <summary>
/// Exposes a cached OpenAPI client instance.
/// </summary>
public interface IZohoOpenApiClientUtil: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<ZohoOpenApiClient> Get(CancellationToken cancellationToken = default);

    /// <summary>Gets a client for a specific API key using the configured base URL.</summary>
    ValueTask<ZohoOpenApiClient> Get(string apiKey, CancellationToken cancellationToken = default);

    /// <summary>Gets a client for a specific Zoho connection.</summary>
    ValueTask<ZohoOpenApiClient> Get(string apiKey, string baseUrl, CancellationToken cancellationToken = default);
}
