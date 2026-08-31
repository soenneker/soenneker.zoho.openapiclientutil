using Soenneker.Zoho.OpenApiClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Zoho.OpenApiClientUtil.Abstract;

/// <summary>
/// Provides cached Zoho CRM OpenAPI clients for access-token and data-center combinations.
/// </summary>
public interface IZohoOpenApiClientUtil: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets a client using the configured access token and CRM API base URL.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<ZohoOpenApiClient> Get(CancellationToken cancellationToken = default);

    /// <summary>Gets a client for a specific access token using the configured base URL.</summary>
    /// <param name="apiKey">The Zoho OAuth access token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The configured Zoho OpenAPI client.</returns>
    ValueTask<ZohoOpenApiClient> Get(string apiKey, CancellationToken cancellationToken = default);

    /// <summary>Gets a client for a specific Zoho access token and CRM API base URL.</summary>
    /// <param name="apiKey">The Zoho OAuth access token.</param>
    /// <param name="baseUrl">The data-center CRM base URL, including the API version.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The configured Zoho OpenAPI client.</returns>
    ValueTask<ZohoOpenApiClient> Get(string apiKey, string baseUrl, CancellationToken cancellationToken = default);
}
