using FamilyHubs.ServiceDirectory.Core.ServiceDirectory.Models;
using FamilyHubs.ServiceDirectory.Web.Filtering.Filters;

namespace FamilyHubs.ServiceDirectory.UnitTests.Filtering;

public class CategoryFilterTests
{
    private readonly ServicesParams _mServicesParams = new("", 0, 0);

    [Fact]
    private void TestFilterNone()
    {
        var filter = new CategoryFilter(TestData.TaxonomyDtos);
        filter.AddFilterCriteria([], _mServicesParams);

        Assert.Equal([], _mServicesParams.TaxonomyIds);
    }

    [Fact]
    private void TestFilter()
    {
        var filter = new CategoryFilter(TestData.TaxonomyDtos);
        filter.AddFilterCriteria(filter.SubFilters.Select(x => x.Aspects.First()), _mServicesParams);

        Assert.Equal(["7", "8", "9", "10", "11", "12"], _mServicesParams.TaxonomyIds);
    }
}
