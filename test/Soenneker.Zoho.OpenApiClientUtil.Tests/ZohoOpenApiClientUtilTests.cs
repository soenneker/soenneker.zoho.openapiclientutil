using Soenneker.Zoho.OpenApiClientUtil.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Zoho.OpenApiClientUtil.Tests;

[Collection("Collection")]
public sealed class ZohoOpenApiClientUtilTests : FixturedUnitTest
{
    private readonly IZohoOpenApiClientUtil _openapiclientutil;

    public ZohoOpenApiClientUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _openapiclientutil = Resolve<IZohoOpenApiClientUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
