using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.Zoho.HttpClients.Abstract;
using Soenneker.Zoho.OpenApiClientUtil.Abstract;
using Soenneker.Zoho.OpenApiClient;
using Soenneker.Kiota.GenericAuthenticationProvider;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.Zoho.OpenApiClientUtil;

///<inheritdoc cref="IZohoOpenApiClientUtil"/>
public sealed class ZohoOpenApiClientUtil : IZohoOpenApiClientUtil
{
    private readonly AsyncSingleton<ZohoOpenApiClient> _client;

    public ZohoOpenApiClientUtil(IZohoOpenApiHttpClient httpClientUtil, IConfiguration configuration)
    {
        _client = new AsyncSingleton<ZohoOpenApiClient>(async token =>
        {
            HttpClient httpClient = await httpClientUtil.Get(token).NoSync();

            var apiKey = configuration.GetValueStrict<string>("Zoho:ApiKey");
            string authHeaderValueTemplate = configuration["Zoho:AuthHeaderValueTemplate"] ?? "Bearer {token}";
            string authHeaderValue = authHeaderValueTemplate.Replace("{token}", apiKey, StringComparison.Ordinal);

            var requestAdapter = new HttpClientRequestAdapter(new GenericAuthenticationProvider(headerValue: authHeaderValue), httpClient: httpClient);

            return new ZohoOpenApiClient(requestAdapter);
        });
    }

    public ValueTask<ZohoOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}
