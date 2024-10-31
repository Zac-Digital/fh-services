using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.UnitTests;

public static class TestData
{
    public static readonly List<TaxonomyDto> TaxonomyDtos = [
        new()
        {
            Id = 1,
            Name = "Activities, clubs and groups"
        },

        new()
        {
            Id = 2,
            Name = "Family support"
        },

        new()
        {
            Id = 3,
            Name = "Health"
        },

        new()
        {
            Id = 4,
            Name = "Pregnancy, birth and early years"
        },

        new()
        {
            Id = 5,
            Name = "Special educational needs and disabilities (SEND)"
        },

        new()
        {
            Id = 6,
            Name = "Transport"
        },

        new()
        {
            Id = 7,
            ParentId = 1,
            Name = "Child 1"
        },

        new()
        {
            Id = 8,
            ParentId = 2,
            Name = "Child 2"
        },

        new()
        {
            Id = 9,
            ParentId = 3,
            Name = "Child 3"
        },

        new()
        {
            Id = 10,
            ParentId = 4,
            Name = "Child 4"
        },

        new()
        {
            Id = 11,
            ParentId = 5,
            Name = "Child 5"
        },

        new()
        {
            Id = 12,
            ParentId = 6,
            Name = "Child 6"
        }
    ];

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
}