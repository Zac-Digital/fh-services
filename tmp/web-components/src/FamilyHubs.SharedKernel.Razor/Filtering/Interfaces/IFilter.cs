using Microsoft.AspNetCore.Http;

namespace FamilyHubs.SharedKernel.Razor.Filtering.Interfaces;

public interface IFilter
{
    public const string RemoveKey = "remove";
    public const string RemoveAllValue = "all";

    string Name { get; }
    string Description { get; }
    string PartialName { get; }

    IEnumerable<IFilterAspect> Aspects { get; }
    IEnumerable<IFilterAspect> SelectedAspects { get; }
    bool IsSelected(IFilterAspect aspect);
}

public interface IFilter<in TFilteringResults> : IFilter
{
    IFilter<TFilteringResults> Apply(IQueryCollection query);

    void AddFilterCriteria(TFilteringResults filteringResults);
    void AddFilterCriteria(IEnumerable<IFilterAspect> selectedAspects, TFilteringResults filteringResults);
}