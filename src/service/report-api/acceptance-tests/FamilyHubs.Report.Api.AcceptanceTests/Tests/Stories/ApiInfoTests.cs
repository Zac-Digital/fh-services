using FamilyHubs.Report.Api.AcceptanceTests.Tests.Steps;
using TestStack.BDDfy;

namespace FamilyHubs.Report.Api.AcceptanceTests.Tests.Stories;

[TestClass]
public class ApiInfoTests
{
    private readonly ApiInfoSteps _steps;

    public ApiInfoTests()
    {
        _steps = new ApiInfoSteps();
    }

    [TestMethod]
    public void Api_Info_Returned()
    {
        this.When(s => _steps.CheckTheApiInfo())
            .Then(s => _steps.AnOkStatusCodeIsReturned())
            .BDDfy();
    }
}