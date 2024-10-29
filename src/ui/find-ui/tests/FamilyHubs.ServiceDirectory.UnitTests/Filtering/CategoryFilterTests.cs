using FamilyHubs.ServiceDirectory.Core.ServiceDirectory.Models;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Web.Filtering.Filters;

namespace FamilyHubs.ServiceDirectory.UnitTests.Filtering;

public class CategoryFilterTests
{
    private readonly ServicesParams _mServicesParams = new("", 0, 0);

    private readonly IList<TaxonomyDto> _taxonomyDtos = new List<TaxonomyDto>
    {
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
    };

    [Fact]
    private void TestFilterNone()
    {
        var filter = new CategoryFilter(_taxonomyDtos);
        filter.AddFilterCriteria([], _mServicesParams);

        Assert.Equal([], _mServicesParams.TaxonomyIds);
    }

    [Fact]
    private void TestFilter()
    {
        var filter = new CategoryFilter(_taxonomyDtos);
        filter.AddFilterCriteria(filter.SubFilters.Select(x => x.Aspects.First()), _mServicesParams);

        Assert.Equal(["7", "8", "9", "10", "11", "12"], _mServicesParams.TaxonomyIds);
    }
}
