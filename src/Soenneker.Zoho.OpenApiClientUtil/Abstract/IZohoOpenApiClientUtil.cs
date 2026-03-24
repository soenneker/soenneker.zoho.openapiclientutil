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
    ValueTask<ZohoOpenApiClient> Get(CancellationToken cancellationToken = default);
}
