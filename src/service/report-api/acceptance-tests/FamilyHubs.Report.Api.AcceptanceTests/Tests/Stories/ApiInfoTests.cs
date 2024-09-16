using FamilyHubs.Report.Api.AcceptanceTests.Tests.Steps;
using TestStack.BDDfy;
using Xunit;

namespace FamilyHubs.Report.Api.AcceptanceTests.Tests.Stories;

public class ApiInfoTests
{
    private readonly ApiInfoSteps _steps = new();

    [Fact]
    public void Api_Info_Returned()
    {
        this.When(s => _steps.CheckTheApiInfo())
            .Then(s => _steps.AnOkStatusCodeIsReturned())
            .BDDfy();
    }
}