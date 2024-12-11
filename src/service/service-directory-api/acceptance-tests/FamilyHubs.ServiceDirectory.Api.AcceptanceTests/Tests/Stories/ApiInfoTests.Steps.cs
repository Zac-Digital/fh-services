using System.Net;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Builders.Http;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Configuration;
using FluentAssertions;
using LightBDD.XUnit2;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Stories;

public partial class ApiInfoTests : FeatureFixture
{
    private readonly string _baseUrl;
    private HttpResponseMessage? _lastResponse;
    
    public ApiInfoTests()
    {
        var config = ConfigAccessor.GetApplicationConfiguration();
        _baseUrl = config.BaseUrl;
    }
    
    private async Task CheckTheApiInfo()
    {
        _lastResponse = await HttpRequestFactory.Get(_baseUrl, "api/info");
    }

    private Task AnOkStatusCodeIsReturned()
    {
        _lastResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
        return Task.CompletedTask;
    }
}