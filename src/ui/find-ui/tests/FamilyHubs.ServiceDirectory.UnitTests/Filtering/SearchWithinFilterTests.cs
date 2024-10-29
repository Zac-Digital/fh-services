using FamilyHubs.ServiceDirectory.Core.ServiceDirectory.Models;
using FamilyHubs.ServiceDirectory.Web.Filtering.Filters;

namespace FamilyHubs.ServiceDirectory.UnitTests.Filtering;

public class SearchWithinFilterTests
{
    private readonly ServicesParams _mServicesParams = new("", 0, 0);

    [Fact]
    private void TestFilterNone()
    {
        var filter = new SearchWithinFilter();
        filter.AddFilterCriteria([], _mServicesParams);

        Assert.Null(_mServicesParams.MaximumProximityMeters);
    }

    [Fact]
    private void TestFilter()
    {
        var filter = new SearchWithinFilter();
        filter.AddFilterCriteria([filter.Aspects.First()], _mServicesParams);

        Assert.Equal(1609, _mServicesParams.MaximumProximityMeters);
    }
}
