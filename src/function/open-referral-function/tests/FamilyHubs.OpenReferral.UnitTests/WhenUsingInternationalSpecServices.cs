using FamilyHubs.OpenReferral.Function.ClientServices;
using FamilyHubs.SharedKernel.OpenReferral.Entities;

namespace FamilyHubs.OpenReferral.UnitTests;

// ! The file service and tests are for the SPIKE/Prototype on Open Referral ingestion from Somerset
// File service is so we do not need to keep hitting the API for testing.
// This way we also abstract the need for http mocking which has no benefit in this case
public class WhenUsingInternationalSpecServices
{
    private readonly DataFileService _fileService = new();

    [Fact]
    public void ShouldDeserializeServices_Services()
    {
        // File includes all or just many of the '/services' endpoint
        var services = _fileService.GetServicesFromFile("services_all.json");

        Assert.NotNull(services);
        Assert.NotEmpty(services);
    }

    [Fact]
    public void ShouldDeserializeServices_Service_Single()
    {
        // File includes a single service
        var services = _fileService.GetServiceFromFile("single.json");

        SingleServiceAssert(services);
    }

    [Fact]
    public void ShouldDeserializeServices_FromListOfSingleServicesFile()
    {
        // Not fully indicative of a real world scenario, but it's a good test to have
        // In RL we would be getting single services 1 by 1 from the API
        // File includes an array of single services
        var services = _fileService.GetSingleServicesFromListFile("single_services_as_list.json");

        Assert.NotNull(services);
        Assert.NotEmpty(services);
        Assert.True(services.First().OrId != Guid.Empty); // Tests converter is converting correctly
    }

    private void SingleServiceAssert(Service? service)
    {
        Assert.NotNull(service);
        Assert.NotNull(service.Organization);
        Assert.NotEqual(service.OrId, Guid.Empty);
    }
}