using FamilyHubs.ServiceDirectory.Core.ServiceDirectory.Models;
using FamilyHubs.ServiceDirectory.Web.Filtering.Filters;

namespace FamilyHubs.ServiceDirectory.UnitTests.Filtering;

public class CostFilterTests
{
    private readonly ServicesParams _mServicesParams = new("", 0, 0);

    [Fact]
    private void TestFilterNone()
    {
        var filter = new CostFilter();
        filter.AddFilterCriteria([], _mServicesParams);

        Assert.Null(_mServicesParams.IsPaidFor);
    }

    [Fact]
    private void TestFilter()
    {
        var filter = new CostFilter();
        filter.AddFilterCriteria([filter.Aspects.First()], _mServicesParams);

        Assert.False(_mServicesParams.IsPaidFor);
    }
}
