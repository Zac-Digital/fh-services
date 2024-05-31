using FamilyHubs.SharedKernel.Razor.Filtering.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FamilyHubs.SharedKernel.Razor.Filtering;

public abstract class Filter<TFilteringResults> : IFilter<TFilteringResults>
{
    protected const string CheckboxesPartialName = "_Checkboxes";
    protected const string RadiosPartialName = "_Radios";

    public string Name { get; }
    public string Description { get; }
    public string PartialName { get; }
    public IEnumerable<IFilterAspect> Aspects { get; }

    private readonly IFilterAspect[] _selectedFilterAspects;

    protected Filter(string name, string description, string partialName, IEnumerable<IFilterAspect> aspects)
    {
        Name = name;
        Description = description;
        PartialName = partialName;
        Aspects = aspects;

        _selectedFilterAspects = Aspects.Where(a => a.SelectedByDefault).ToArray();
    }

    public virtual IFilter<TFilteringResults> Apply(IQueryCollection query)
    {
        return new AppliedFilter<TFilteringResults>(this, query);
    }

    public IEnumerable<IFilterAspect> SelectedAspects => _selectedFilterAspects;

    public bool IsSelected(IFilterAspect aspect)
    {
        return aspect.SelectedByDefault;
    }

    public void AddFilterCriteria(TFilteringResults filteringResults)
    {
        AddFilterCriteria(SelectedAspects, filteringResults);
    }

    public abstract void AddFilterCriteria(IEnumerable<IFilterAspect> selectedAspects, TFilteringResults filteringResults);
}