using Xunit;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Steps;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Stories;

/// <summary>
/// As a UI service
/// I want to be able to check the API health
/// So that I can send calls to the API to get or manage data 
/// </summary>
public class ApiInfoTests
{
    private readonly ApiInfoSteps _steps = new();

    [Fact]
    public async Task Api_Info_Returned()
    {
        await _steps.CheckTheApiInfo();
        _steps.AnOkStatusCodeIsReturned();
    }
}