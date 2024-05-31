using FamilyHubs.SharedKernel.Razor.Filtering.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace FamilyHubs.SharedKernel.Razor.Filtering;

public class AppliedFilterSubGroups<TFilteringResults> : IFilterSubGroups<TFilteringResults>
{
    public string Name => _filterSubGroups.Name;
    public string Description => _filterSubGroups.Description;
    public string PartialName => _filterSubGroups.PartialName;
    public IEnumerable<IFilter<TFilteringResults>> SubFilters { get; }
    // makes sense, but no current consumers
    public IEnumerable<IFilterAspect> Aspects => throw new NotImplementedException();
    public IEnumerable<IFilterAspect> SelectedAspects { get; }

    private readonly FilterSubGroups<TFilteringResults> _filterSubGroups;

    public AppliedFilterSubGroups(FilterSubGroups<TFilteringResults> filterSubGroups, IQueryCollection query)
    {
        _filterSubGroups = filterSubGroups;

        SubFilters = filterSubGroups.SubFilters.Select(f => new AppliedFilter<TFilteringResults>(f, query)).ToArray();
        SelectedAspects = SubFilters.SelectMany(f => f.SelectedAspects);
    }

    //todo: covariance
    //    public IFilterSubGroups Apply(IQueryCollection query)
    public IFilter<TFilteringResults> Apply(IQueryCollection query)
    {
        Debug.Assert(false, "Calling Apply() on a AppliedFilter");
        return this;
    }

    public virtual void AddFilterCriteria(TFilteringResults filteringResults)
    {
        _filterSubGroups.AddFilterCriteria(SelectedAspects, filteringResults);
    }

    public void AddFilterCriteria(IEnumerable<IFilterAspect> selectedAspects, TFilteringResults filteringResults)
    {
        _filterSubGroups.AddFilterCriteria(selectedAspects, filteringResults);
    }

    // makes sense, but no current consumers
    public bool IsSelected(IFilterAspect aspect)
    {
        throw new NotImplementedException();
    }
}