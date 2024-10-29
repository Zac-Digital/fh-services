using FamilyHubs.ServiceDirectory.Core.ServiceDirectory.Models;
using FamilyHubs.ServiceDirectory.Web.Filtering.Filters;

namespace FamilyHubs.ServiceDirectory.UnitTests.Filtering;

public class ChildrenAndYoungPeopleFilterTests
{
    private readonly ServicesParams _mServicesParams = new("", 0, 0);

    [Fact]
    private void TestFilterNone()
    {
        var filter = new ChildrenAndYoungPeopleFilter();
        filter.AddFilterCriteria([], _mServicesParams);

        Assert.Null(_mServicesParams.AllChildrenYoungPeople);
        Assert.Null(_mServicesParams.GivenAge);
    }

    [Fact]
    private void TestFilterAll()
    {
        var filter = new ChildrenAndYoungPeopleFilter();
        filter.AddFilterCriteria([filter.Aspects.First()], _mServicesParams);

        Assert.Equal(true, _mServicesParams.AllChildrenYoungPeople);
        Assert.Null(_mServicesParams.GivenAge);
    }
    
    [Fact]
    private void TestFilter()
    {
        var filter = new ChildrenAndYoungPeopleFilter();
        foreach (var filterAspect in filter.Aspects.Skip(1))
        {
            filter.AddFilterCriteria([filterAspect], _mServicesParams);

            Assert.NotEqual(true, _mServicesParams.AllChildrenYoungPeople);
            Assert.Equal(int.Parse(filterAspect.Id), _mServicesParams.GivenAge);
        }
    }
}
