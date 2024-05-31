using FamilyHubs.SharedKernel.Razor.Filtering.Interfaces;

namespace FamilyHubs.SharedKernel.Razor.Filtering;

public class SubFilter<TFilteringResults> : Filter<TFilteringResults>
{
    public SubFilter(string name, string description, IEnumerable<IFilterAspect> aspects)
        : base(name, description, CheckboxesPartialName, aspects)
    {
    }

    public override void AddFilterCriteria(IEnumerable<IFilterAspect> selectedAspects, TFilteringResults filteringResults)
    {
        // handled by FilterSubGroups
        throw new NotImplementedException();
    }
}