
namespace FamilyHubs.SharedKernel.Razor.Filtering.Interfaces;

public interface IFilterSubGroups : IFilter
{
    IEnumerable<IFilter> SubFilters { get; }
}

//todo: interface inheritance may be wrong here
public interface IFilterSubGroups<in TFilteringResults> : IFilter<TFilteringResults>
{
    IEnumerable<IFilter<TFilteringResults>> SubFilters { get; }
}