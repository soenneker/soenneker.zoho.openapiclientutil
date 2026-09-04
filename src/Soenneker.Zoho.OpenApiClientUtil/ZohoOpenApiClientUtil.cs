using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Dictionaries.Singletons;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.Zoho.HttpClients.Abstract;
using Soenneker.Zoho.OpenApiClientUtil.Abstract;
using Soenneker.Zoho.OpenApiClient;

namespace Soenneker.Zoho.OpenApiClientUtil;

/// <inheritdoc cref="IZohoOpenApiClientUtil" />
public sealed class ZohoOpenApiClientUtil : IZohoOpenApiClientUtil
{
    private readonly SingletonDictionary<ZohoOpenApiClient> _clients;
    private readonly IZohoOpenApiHttpClient _httpClientUtil;
    private readonly IConfiguration _configuration;
    private readonly string _baseUrl;

    public ZohoOpenApiClientUtil(IZohoOpenApiHttpClient httpClientUtil, IConfiguration configuration)
    {
        _httpClientUtil = httpClientUtil;
        _configuration = configuration;
        _baseUrl = configuration["Zoho:ClientBaseUrl"] ?? "https://www.zohoapis.com/crm/v8/";
        _clients = new SingletonDictionary<ZohoOpenApiClient>(CreateClient);
    }

    private async ValueTask<ZohoOpenApiClient> CreateClient(string connectionKey, CancellationToken token)
    {
        (string accessToken, string baseUrl) = ParseConnectionKey(connectionKey);
        HttpClient httpClient = await _httpClientUtil.Get(accessToken, baseUrl, token).NoSync();

        var requestAdapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: httpClient)
        {
            BaseUrl = baseUrl
        };

        return new ZohoOpenApiClient(requestAdapter);
    }

    public ValueTask<ZohoOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        string accessToken = _configuration["Zoho:AccessToken"] ?? _configuration.GetValueStrict<string>("Zoho:ApiKey");
        return Get(accessToken, _baseUrl, cancellationToken);
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

    private static (string AccessToken, string BaseUrl) ParseConnectionKey(string connectionKey)
    {
        int separatorIndex = connectionKey.IndexOf('\0');

        return (connectionKey[..separatorIndex], connectionKey[(separatorIndex + 1)..]);
    }

    public void Dispose()
    {
        _clients.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _clients.DisposeAsync();
    }
}
