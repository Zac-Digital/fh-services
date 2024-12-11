using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Stories;

[FeatureDescription(
    """
    In order for the UI to call APIs
    As a UI service
    I want to be able to get API health status from the API
    """)]
public partial class ApiInfoTests
{
    [Scenario]
    public async Task Api_Info_Returned()
    {
        await Runner.RunScenarioAsync(
            _ => CheckTheApiInfo(), 
            _ => AnOkStatusCodeIsReturned());
    }
}