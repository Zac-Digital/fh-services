using FamilyHubs.SharedKernel.Razor.Filtering.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FamilyHubs.SharedKernel.Razor.Filtering;

public abstract class FilterSubGroups<TFilteringResults> : IFilterSubGroups<TFilteringResults>
{
    public string Name { get; }
    public string Description { get; }
    public string PartialName => "_SubGroups";
    public IEnumerable<IFilter<TFilteringResults>> SubFilters { get; }
    public IEnumerable<IFilterAspect> Aspects { get; }
    public IEnumerable<IFilterAspect> SelectedAspects { get; }

    protected FilterSubGroups(string name, string description, IEnumerable<IFilter<TFilteringResults>> subFilters)
    {
        Name = name;
        Description = description;
        SubFilters = subFilters as IFilter<TFilteringResults>[] ?? subFilters.ToArray();
        Aspects = SubFilters.SelectMany(f => f.Aspects);
        SelectedAspects = SubFilters.SelectMany(f => f.SelectedAspects);
    }

    //todo: covariance
    //    public IFilterSubGroups Apply(IQueryCollection query)
    public IFilter<TFilteringResults> Apply(IQueryCollection query)
    {
        return new AppliedFilterSubGroups<TFilteringResults>(this, query);
    }

    public void AddFilterCriteria(TFilteringResults filteringResults)
    {
        AddFilterCriteria(SelectedAspects, filteringResults);
    }

    public abstract void AddFilterCriteria(IEnumerable<IFilterAspect> selectedAspects, TFilteringResults filteringResults);

    // makes sense, but no current consumers
    public bool IsSelected(IFilterAspect aspect)
    {
        throw new NotImplementedException();
    }
}