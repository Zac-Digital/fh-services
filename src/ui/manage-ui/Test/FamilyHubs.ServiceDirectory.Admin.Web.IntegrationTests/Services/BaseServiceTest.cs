using System.Text;
using System.Text.Json;
using FamilyHubs.ServiceDirectory.Admin.Core.ApiClient;
using FamilyHubs.ServiceDirectory.Admin.Core.Models;
using FamilyHubs.ServiceDirectory.Admin.Core.Models.ServiceJourney;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Models;
using FamilyHubs.SharedKernel.Razor.Dashboard;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace FamilyHubs.ServiceDirectory.Admin.Web.IntegrationTests.Services;

public class BaseServiceTest : BaseTest
{
    private readonly IDistributedCache _cache = Substitute.For<IDistributedCache>();
    private readonly ITaxonomyService _taxonomyService = Substitute.For<ITaxonomyService>();
    private readonly IServiceDirectoryClient _serviceDirectoryClient = Substitute.For<IServiceDirectoryClient>();
    private readonly ServiceModel<object> _fakeService = new()
    {
        Id = 1,
        Name = "Service Name",
        EntryPoint = ServiceDetailEntrance.FromManageServicesPage,
        Description = "Service Summary",
        MoreDetails = "Service Description",
        ServiceType = ServiceTypeArg.Vcs,
        OrganisationId = 1,
        LaOrganisationId = 2,
        ForChildren = true,
        MinimumAge = 5,
        MaximumAge = 25,
        HasTelephone = true,
        TelephoneNumber = "01234567890",
        HasCost = false,
        HasTimeDetails = false,
        LanguageCodes = ["en"],
        SelectedSubCategories = [2],
        HowUse = [AttendingType.InPerson, AttendingType.Online],
        Locations = [
            new ServiceLocationModel(1, ["Address First Line", "Address Second Line"], "Location 1", false, "Location description")
            {
                HasTimeDetails = false
            }
        ]
    };
    private readonly List<OrganisationDetailsDto> _organisationDetails = [
        new()
        {
            Id = 1,
            Name = "Test Org",
            OrganisationType = OrganisationType.VCFS,
            Description = "Test org description",
            AdminAreaCode = "ADM0001"
        },
        new ()
        {
            Id = 2,
            Name = "Test LA Org",
            OrganisationType = OrganisationType.LA,
            Description = "Test LA org description",
            AdminAreaCode = "ADM0002"
        }
    ];

    public BaseServiceTest()
    {
        var fakeServiceBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(_fakeService));
        _cache.GetAsync(Arg.Any<string>()).Returns(fakeServiceBytes);

        _taxonomyService.GetCategories(Arg.Any<CancellationToken>())
            .Returns([
                KeyValuePair.Create(new TaxonomyDto { Id = 1, Name = "Base Taxonomy" }, new List<TaxonomyDto> { new() { Id = 2, Name = "Sub Taxonomy" } })
            ]);

        _serviceDirectoryClient.GetOrganisations(Arg.Any<CancellationToken>(), Arg.Any<OrganisationType?>(), Arg.Any<long?>())
            .Returns(_organisationDetails.OfType<OrganisationDto>().ToList());
        foreach (var organisationDto in _organisationDetails)
        {
            _serviceDirectoryClient.GetOrganisationById(organisationDto.Id, Arg.Any<CancellationToken>())
                .Returns(organisationDto);
        }

        _serviceDirectoryClient.GetServiceSummaries(Arg.Any<long?>(), Arg.Any<string?>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<SortOrder>(), Arg.Any<CancellationToken>())
            .Returns(new PaginatedList<ServiceNameDto>());

        _serviceDirectoryClient.GetLocations(Arg.Any<bool>(), Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<bool>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(new PaginatedList<LocationDto>());
    }

    protected override void Configure(IServiceCollection services)
    {
        services.AddSingleton(_cache);
        services.AddSingleton(_serviceDirectoryClient);
        services.AddSingleton(_taxonomyService);
    }
}
