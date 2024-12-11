using System.Net;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Steps;
using Xunit;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Stories;

/// <summary>
/// As a user of the metrics api
/// I want to be able to update usage information for postcode searches and filters
/// So that I can record how search functionality is being used
/// </summary>
public class ServiceSearchTests
{
    private readonly ServiceSearchSteps _steps = new();
    
    [Theory]
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
        _steps.GivenIHaveASearchServiceRequest(radius, postcode, statusCode, searchTriggerEventId, serviceSearchTypeId);
        await _steps.WhenISendARequest();
        _steps.ThenExpectedStatusCodeReturned(HttpStatusCode.OK);
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
        _steps.GivenIHaveASearchServiceRequest(radius, postcode, statusCode, searchTriggerEventId, serviceSearchTypeId);
        await _steps.WhenISendARequest();
        _steps.ThenExpectedStatusCodeReturned(HttpStatusCode.InternalServerError);
    }
}