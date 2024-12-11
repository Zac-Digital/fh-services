using System.Net;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Builders.Http;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Configuration;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Models;
using FluentAssertions;
using LightBDD.XUnit2;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Stories;

public partial class ServiceSearchTests : FeatureFixture
{
    private readonly string _baseUrl = ConfigAccessor.GetApplicationConfiguration().BaseUrl;
    private ServiceSearchRequest _request = new();
    private HttpResponseMessage _lastResponse = new();
    private HttpStatusCode _statusCode;
    private const string ServiceSearchPath = "api/metrics/service-search";

    private Task GivenIHaveASearchServiceRequest(
        string radiusValue, 
        string postcodeEntry,
        string postCodeEndpointResponseEntry,
        string searchTriggerEventId,
        string serviceSearchTypeId)
    {
        var time = DateTime.UtcNow;
        var radius = int.Parse(radiusValue);
        var postcodeEndpointStatusCode = int.Parse(postCodeEndpointResponseEntry);
        var searchTriggerEventIdEntry = int.Parse(searchTriggerEventId);
        var serviceSearchTypeIdEntry = int.Parse(serviceSearchTypeId);
        _request = new ServiceSearchRequest
        {
            SearchPostcode = postcodeEntry,
            SearchRadiusMiles = radius,
            UserId = 0,
            HttpResponseCode = postcodeEndpointStatusCode,
            RequestTimestamp = time,
            ResponseTimestamp = time,
            CorrelationId = "",
            SearchTriggerEventId = searchTriggerEventIdEntry,
            ServiceSearchTypeId = serviceSearchTypeIdEntry,
            ServiceSearchResults = new List<ServiceSearchResults>()
        };
        return Task.CompletedTask;
    }

    private async Task<HttpStatusCode> WhenISendARequest()
    {
        _lastResponse = await HttpRequestFactory.Post(_baseUrl, ServiceSearchPath, _request);
        _statusCode = _lastResponse.StatusCode;
        return _statusCode;
    }

    private Task ThenExpectedStatusCodeReturned(HttpStatusCode expectedStatusCode)
    {
        _statusCode.Should().Be(expectedStatusCode,
            ResponseNotExpectedMessage(
                _lastResponse.RequestMessage!.Method,
                _lastResponse.RequestMessage.RequestUri!,
                _statusCode));
        return Task.CompletedTask;
    }

    private static string ResponseNotExpectedMessage(HttpMethod method, Uri requestUri, HttpStatusCode statusCode)
    {
        return $"Response from {method} {requestUri} {statusCode} was not as expected";
    }
}