using System.Net;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using Xunit;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Stories;

[FeatureDescription(
    """
    In order to update usage information for postcode searches and filters
    As a user of the metrics api
    I want to be able to record how search functionality is being used
    """)]
public partial class ServiceSearchTests
{
    [Scenario]
    [InlineData("20", "E1 2EN", "200", "1", "2")] // As a Find user
    [InlineData("5", "E1 2EN", "401", "2", "2")] // As a Find user subsequent search
    [InlineData("20", "E1 2EN", "500", "1", "1")] // As a Connect user
    [InlineData("0", "E1 2EN", "400", "2", "1")] // As a Connect user subsequent search
    public async Task Service_Search_Metrics_Endpoint_Returns_A_Status_Code_Ok(
        string radius, 
        string postcode, 
        string statusCode, 
        string searchTriggerEventId, 
        string serviceSearchTypeId)
    {
        await Runner.RunScenarioAsync(
            _ => GivenIHaveASearchServiceRequest(radius, postcode, statusCode, searchTriggerEventId, serviceSearchTypeId),
            _ => WhenISendARequest(),
            _ => ThenExpectedStatusCodeReturned(HttpStatusCode.OK));
    }
    
    [Theory]
    [InlineData("15", "E1 2EN", "200", "3", "1")] // Invalid Search Trigger Event Type
    [InlineData("15", "E1 2EN", "200", "1", "3")] // Invalid ServiceSearchTypeId
    public async Task Service_Search_Metrics_Endpoint_Returns_Status_Code_Internal_Server_Error(
        string radius,
        string postcode,
        string statusCode,
        string searchTriggerEventId,
        string serviceSearchTypeId)
    {
        await Runner.RunScenarioAsync(
            _ => GivenIHaveASearchServiceRequest(radius, postcode, statusCode, searchTriggerEventId, serviceSearchTypeId),
            _ => WhenISendARequest(),
            _ => ThenExpectedStatusCodeReturned(HttpStatusCode.InternalServerError));
    }
}