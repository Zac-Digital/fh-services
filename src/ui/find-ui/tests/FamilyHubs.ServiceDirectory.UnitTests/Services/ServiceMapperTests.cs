using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Web.Mappers;
using FamilyHubs.ServiceDirectory.Web.Models;
using ServiceType = FamilyHubs.ServiceDirectory.Web.Models.ServiceType;

namespace FamilyHubs.ServiceDirectory.UnitTests.Services;

public class ServiceMapperTests
{
    public static readonly List<ServiceDto> ExampleServices = [
        new()
        {
            Id = 1,
            Name = "ExampleService",
            ServiceType = Shared.Enums.ServiceType.FamilyExperience,
            Distance = 10_000d,
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
                    Address1 = "ExampleAddress",
                    City = "ExampleCity",
                    PostCode = "ExamplePostCode",
                    StateProvince = "ExampleStateProvince",
                    Country = "ExampleCountry",
                    LocationType = LocationType.Physical
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
            Taxonomies = new List<TaxonomyDto>
            {
                new()
                {
                    Id = 3,
                    ParentId = 2,
                    Name = "C"
                },
                new()
                {
                    Id = 2,
                    ParentId = 2,
                    Name = "B"
                },
                new()
                {
                    Id = 4,
                    ParentId = 1,
                    Name = "A"
                }
            }
        },
        new()
        {
            Id = 1,
            Name = "ExampleService2",
            ServiceType = Shared.Enums.ServiceType.FamilyExperience,
            Locations = new List<LocationDto>
            {
                new()
                {
                    LocationTypeCategory = LocationTypeCategory.FamilyHub,
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
            CostOptions = new List<CostOptionDto>
            {
                new()
                {
                    AmountDescription = "Information."
                }
            }
        }
    ];
    
    [Fact]
    private void ToViewModel()
    {
        var expected = new List<Service>
        {
            new(
                ServiceType.Service,
                "ExampleService",
                6.2137273664980679d,
                ["Free"],
                ["ExampleAddress", "ExampleCity", "ExampleStateProvince", "ExamplePostCode"],
                [],
                ["A", "B", "C"],
                "18 to 65",
                "01234567890",
                "email@example.com",
                "ExampleService",
                "http://example.com"
            ),
            new(
                ServiceType.FamilyHub,
                "ExampleService2",
                null,
                ["Yes, it costs money to use. Information."],
                ["ExampleAddress2", "ExampleCity2", "ExampleStateProvince2", "ExamplePostCode2"],
                [],
                [],
                null,
                null,
                null,
                "ExampleService2",
                null
            )
        };

        var result = ServiceMapper.ToViewModel(ExampleServices);
        Assert.Equal(expected, result);
    }
}
