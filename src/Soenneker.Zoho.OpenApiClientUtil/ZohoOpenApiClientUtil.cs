using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Dictionaries.Singletons;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.Zoho.HttpClients.Abstract;
using Soenneker.Zoho.OpenApiClientUtil.Abstract;
using Soenneker.Zoho.OpenApiClient;
using Soenneker.Kiota.GenericAuthenticationProvider;

namespace Soenneker.Zoho.OpenApiClientUtil;

///<inheritdoc cref="IZohoOpenApiClientUtil"/>
public sealed class ZohoOpenApiClientUtil : IZohoOpenApiClientUtil
{
    private readonly SingletonDictionary<ZohoOpenApiClient> _clients;
    private readonly IZohoOpenApiHttpClient _httpClientUtil;
    private readonly string _apiKey;
    private readonly string _baseUrl;
    private readonly string _authHeaderName;
    private readonly string _authHeaderValueTemplate;

    public ZohoOpenApiClientUtil(IZohoOpenApiHttpClient httpClientUtil, IConfiguration configuration)
    {
        _httpClientUtil = httpClientUtil;
        _apiKey = configuration.GetValueStrict<string>("Zoho:ApiKey");
        _baseUrl = configuration["Zoho:ClientBaseUrl"] ?? "https://zohoapis.com/crm/8.0";
        _authHeaderName = configuration["Zoho:AuthHeaderName"] ?? "Authorization";
        _authHeaderValueTemplate = configuration["Zoho:AuthHeaderValueTemplate"] ?? "Bearer {token}";
        _clients = new SingletonDictionary<ZohoOpenApiClient>(CreateClient);
    }

    private async ValueTask<ZohoOpenApiClient> CreateClient(string connectionKey, CancellationToken token)
    {
        (string apiKey, string baseUrl) = ParseConnectionKey(connectionKey);
        HttpClient httpClient = await _httpClientUtil.Get(token).NoSync();
        string authHeaderValue = _authHeaderValueTemplate.Replace("{token}", apiKey, StringComparison.Ordinal);

        var requestAdapter = new HttpClientRequestAdapter(
            new GenericAuthenticationProvider(headerName: _authHeaderName, headerValue: authHeaderValue), httpClient: httpClient)
        {
            BaseUrl = baseUrl
        };

        return new ZohoOpenApiClient(requestAdapter);
    }

    public ValueTask<ZohoOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return Get(_apiKey, _baseUrl, cancellationToken);
    }

    public ValueTask<ZohoOpenApiClient> Get(string apiKey, CancellationToken cancellationToken = default)
    {
        return Get(apiKey, _baseUrl, cancellationToken);
    }

    public ValueTask<ZohoOpenApiClient> Get(string apiKey, string baseUrl, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);

        string normalizedBaseUrl = new Uri(baseUrl, UriKind.Absolute).AbsoluteUri.TrimEnd('/');

        return _clients.Get(CreateConnectionKey(apiKey, normalizedBaseUrl), cancellationToken);
    }

    private static string CreateConnectionKey(string apiKey, string baseUrl) => string.Concat(apiKey, "\0", baseUrl);

    private static (string ApiKey, string BaseUrl) ParseConnectionKey(string connectionKey)
    {
        int separatorIndex = connectionKey.IndexOf('\0');

        return (connectionKey[..separatorIndex], connectionKey[(separatorIndex + 1)..]);
    }

    /// <summary>
    /// Releases resources used by the current instance.
    /// </summary>
    public void Dispose()
    {
        _clients.Dispose();
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public ValueTask DisposeAsync()
    {
        return _clients.DisposeAsync();
    }
}
