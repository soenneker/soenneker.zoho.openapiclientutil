[![](https://img.shields.io/nuget/v/soenneker.zoho.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.zoho.openapiclientutil/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.zoho.openapiclientutil/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.zoho.openapiclientutil/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.zoho.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.zoho.openapiclientutil/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.zoho.openapiclientutil/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.zoho.openapiclientutil/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Zoho.OpenApiClientUtil

Provides cached `ZohoOpenApiClient` instances for access-token and data-center combinations.

## Installation

```shell
dotnet add package Soenneker.Zoho.OpenApiClientUtil
```

## Configuration

```json
{
  "Zoho": {
    "AccessToken": "your-oauth-access-token",
    "ClientBaseUrl": "https://www.zohoapis.com/crm/v8/"
  }
}
```

The US CRM v8 base URL is used when `ClientBaseUrl` is omitted. Use the `api_domain` returned by Zoho's token endpoint for other data centers. `Zoho:ApiKey` remains supported as a legacy alias for `Zoho:AccessToken`.

## Registration

```csharp
services.AddZohoOpenApiClientUtilAsSingleton();
```

Scoped registration is also available:

```csharp
services.AddZohoOpenApiClientUtilAsScoped();
```

The scoped provider borrows the singleton HTTP transport. Disposing it releases its generated clients without removing the shared transport.

## Usage

```csharp
public sealed class ZohoUserReader
{
    private readonly IZohoOpenApiClientUtil _zoho;

    public ZohoUserReader(IZohoOpenApiClientUtil zoho)
    {
        _zoho = zoho;
    }

    public async Task PrintUsers(CancellationToken cancellationToken)
    {
        ZohoOpenApiClient client = await _zoho.Get(cancellationToken);
        var response = await client.Users.GetAsync(cancellationToken: cancellationToken);

        foreach (var user in response?.Users ?? [])
            Console.WriteLine($"{user.FullName} ({user.Email})");
    }
}
```

Pass connection values explicitly when serving multiple Zoho tenants or data centers:

```csharp
ZohoOpenApiClient tenantClient = await zohoClientUtil.Get(
    tenantAccessToken,
    "https://www.zohoapis.eu/crm/v8/",
    cancellationToken);
```

The provider caches one generated client per access-token/base-URL pair. It does not obtain or refresh OAuth tokens.
