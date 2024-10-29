using FamilyHubs.ServiceDirectory.Core.ServiceDirectory.Interfaces;
using FamilyHubs.ServiceDirectory.Core.ServiceDirectory.Models;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace FamilyHubs.ServiceDirectory.UnitTests.Web;

// TODO: Move to integration test project??
public class IndexUiTests : BaseUiTest
{
    private readonly IServiceDirectoryClient _serviceDirectoryClient = Substitute.For<IServiceDirectoryClient>();

    protected override void Configure(IServiceCollection services)
    {
        services.AddSingleton(_serviceDirectoryClient);
    }

    [Fact]
    public async Task Demo()
    {
        // Act
        var page = await Navigate("/");

        // Assert
        var startButton = page.QuerySelector("[data-testid=\"start-button\"]");
        Assert.Equal("Start now", startButton?.TextContent.Trim());
    }

    [Fact]
    public async Task ServiceFilter_NoResults()
    {
        _serviceDirectoryClient.GetTaxonomies(Arg.Any<CancellationToken>()).Returns(new PaginatedList<TaxonomyDto>());
        _serviceDirectoryClient.GetServices(Arg.Any<ServicesParams>()).Returns(
            (new PaginatedList<ServiceDto>([], 0, 1, 10), null)
        );

        // Act
        var page = await Navigate("/ServiceFilter?postcode=W1A 1AA&adminarea=E09000033&latitude=51.518562&longitude=-0.143799&frompostcodesearch=true");

        // Assert
        var noResultsHeading = page.QuerySelector("[data-testid=\"no-results-heading\"]");
        Assert.NotNull(noResultsHeading);
    }

    [Fact]
    public async Task ServiceFilter_NoResultsFiltered()
    {
        _serviceDirectoryClient.GetTaxonomies(Arg.Any<CancellationToken>()).Returns(new PaginatedList<TaxonomyDto>());
        _serviceDirectoryClient.GetServices(Arg.Any<ServicesParams>()).Returns(
            (new PaginatedList<ServiceDto>([], 0, 1, 10), null)
        );

        // Act
        var page = await Navigate("/ServiceFilter?postcode=W1A 1AA&adminarea=E09000033&latitude=51.518562&longitude=-0.143799");

        // Assert
        var noResultsHeading = page.QuerySelector("[data-testid=\"empty-filters-heading\"]");
        Assert.NotNull(noResultsHeading);
    }

    [Fact]
    public async Task ServiceFilter_Results()
    {
        _serviceDirectoryClient.GetTaxonomies(Arg.Any<CancellationToken>()).Returns(new PaginatedList<TaxonomyDto>([
            new TaxonomyDto
            {
                Id = 1,
                Name = "Activities, clubs and groups"
            },
            new TaxonomyDto
            {
                Id = 2,
                Name = "Family support"
            },
            new TaxonomyDto
            {
                Id = 3,
                Name = "Health"
            },
            new TaxonomyDto
            {
                Id = 4,
                Name = "Pregnancy, birth and early years"
            },
            new TaxonomyDto
            {
                Id = 5,
                Name = "Special educational needs and disabilities (SEND)"
            },
            new TaxonomyDto
            {
                Id = 6,
                Name = "Transport"
            },
            new TaxonomyDto
            {
                Id = 7,
                ParentId = 1,
                Name = "Test"
            }
        ], 1, 1, 10));
        _serviceDirectoryClient.GetServices(Arg.Any<ServicesParams>()).Returns(
            (new PaginatedList<ServiceDto>([
                new ServiceDto
                {
                    Id = 1,
                    Name = "ExampleService2",
                    ServiceType = ServiceType.FamilyExperience,
                    Eligibilities = new List<EligibilityDto>
                    {
                        new()
                        {
                            MinimumAge = 18,
                            MaximumAge = 65
                        }
                    },
                    Locations = new List<LocationDto>
                    {
                        new()
                        {
                            LocationTypeCategory = LocationTypeCategory.NotSet,
                            Latitude = 51,
                            Longitude = -1,
                            Address1 = "ExampleAddress2",
                            City = "ExampleCity2",
                            PostCode = "ExamplePostCode2",
                            StateProvince = "ExampleStateProvince2",
                            Country = "ExampleCountry2",
                            LocationType = LocationType.Virtual
                        }
                    },
                    ServiceDeliveries = new List<ServiceDeliveryDto>
                    {
                        new()
                        {
                            Name = AttendingType.InPerson
                        }
                    },
                    Contacts = new List<ContactDto>
                    {
                        new()
                        {
                            Email = "email@example.com",
                            Telephone = "01234567890",
                            Url = "example.com"
                        }
                    },
                    CostOptions = new List<CostOptionDto>
                    {
                        new()
                        {
                            AmountDescription = "Information."
                        }
                    }
                }
            ], 25, 2, 10), null)
        );
        
        // Act
        var page = await Navigate("/ServiceFilter?postcode=W1A 1AA&adminarea=E09000033&latitude=51.518562&longitude=-0.143799&search_within=20&activities=7");

        // Assert
        var serviceEntries = page.QuerySelectorAll("[data-testid=\"service-entry\"]");
        Assert.Equal(1, serviceEntries.Length);
    }
}
