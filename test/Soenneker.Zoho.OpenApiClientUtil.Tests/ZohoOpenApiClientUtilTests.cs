using Soenneker.Zoho.OpenApiClientUtil.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Zoho.OpenApiClientUtil.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class ZohoOpenApiClientUtilTests : HostedUnitTest
{
    private readonly IZohoOpenApiClientUtil _openapiclientutil;

    public ZohoOpenApiClientUtilTests(Host host) : base(host)
    {
        _openapiclientutil = Resolve<IZohoOpenApiClientUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
