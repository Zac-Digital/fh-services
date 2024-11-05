using FamilyHubs.Referral.Core.ClientServices;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyHubs.Referral.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingServiceDirectoryApiTests : BaseWhenUsingOpenReferralApiUnitTests
{
    [Fact]
    public async Task ThenGetOrganisationByIdIsRetrieved()
    {
        using var scope = WebAppFactory!.Services.CreateScope();
        var serviceDirectoryService = scope.ServiceProvider.GetRequiredService<IServiceDirectoryService>();

        var result = await serviceDirectoryService.GetOrganisationById(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);

    }

    [Fact]
    public async Task ThenGetServiceByIdIsRetrieved()
    {
        using var scope = WebAppFactory!.Services.CreateScope();
        var serviceDirectoryService = scope.ServiceProvider.GetRequiredService<IServiceDirectoryService>();

        var result = await serviceDirectoryService.GetServiceById(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }
}
