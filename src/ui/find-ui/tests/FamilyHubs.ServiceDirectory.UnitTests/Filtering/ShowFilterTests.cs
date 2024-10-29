using FamilyHubs.ServiceDirectory.Core.ServiceDirectory.Models;
using FamilyHubs.ServiceDirectory.Web.Filtering.Filters;

namespace FamilyHubs.ServiceDirectory.UnitTests.Filtering;

public class ShowFilterTests
{
    private readonly ServicesParams _mServicesParams = new("", 0, 0);

    [Fact]
    private void TestFilterNone()
    {
        var filter = new ShowFilter();
        filter.AddFilterCriteria([], _mServicesParams);

        Assert.Null(_mServicesParams.FamilyHub);
    }

    [Fact]
    private void TestFilter()
    {
        var filter = new ShowFilter();
        filter.AddFilterCriteria([filter.Aspects.First()], _mServicesParams);

        Assert.True(_mServicesParams.FamilyHub);
    }
}
