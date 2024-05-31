using FamilyHubs.SharedKernel.Razor.Filtering.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace FamilyHubs.SharedKernel.Razor.Filtering;

public class AppliedFilter<TFilteringResults> : IFilter<TFilteringResults>
{
    public string Name => Filter.Name;
    public string Description => Filter.Description;
    public string PartialName => Filter.PartialName;
    public IEnumerable<IFilterAspect> Aspects => Filter.Aspects;
    public IEnumerable<IFilterAspect> SelectedAspects { get; protected set; }

    protected readonly IFilter<TFilteringResults> Filter;

    public AppliedFilter(IFilter<TFilteringResults> filter, IQueryCollection query)
    {
        Filter = filter;

        string? fullValuesCsv = query[filter.Name];
        if (fullValuesCsv != null)
        {
            string[] fullValues = fullValuesCsv.Split(',');

            SelectedAspects = Filter.Aspects.Where(a => fullValues.Contains(a.Value)).ToArray();
        }
        else
        {
            SelectedAspects = Array.Empty<IFilterAspect>();
        }
    }

    public bool IsSelected(IFilterAspect aspect)
    {
        return SelectedAspects.Any(a => a.Id == aspect.Id);
    }

    public IFilter<TFilteringResults> Apply(IQueryCollection query)
    {
        Debug.Assert(false, "Calling Apply() on an AppliedFilter");
        return this;
    }

    public void AddFilterCriteria(TFilteringResults filteringResults)
    {
        Filter.AddFilterCriteria(SelectedAspects, filteringResults);
    }

    public void AddFilterCriteria(IEnumerable<IFilterAspect> selectedAspects, TFilteringResults filteringResults)
    {
        Filter.AddFilterCriteria(selectedAspects, filteringResults);
    }
}