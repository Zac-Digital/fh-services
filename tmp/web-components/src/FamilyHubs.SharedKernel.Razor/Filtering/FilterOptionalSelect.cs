using FamilyHubs.SharedKernel.Razor.Filtering.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FamilyHubs.SharedKernel.Razor.Filtering;

public abstract class FilterOptionalSelect<TFilteringResults> : Filter<TFilteringResults>, IFilterOptionalSelect<TFilteringResults>
{
    public bool IsOptionSelected { get; }
    public string OptionDescription { get; }
    public string SelectDescription { get; }
    public string OptionSelectedName { get; }

    protected FilterOptionalSelect(
        string name,
        string description,
        string optionDescription,
        string selectDescription,
        IEnumerable<IFilterAspect> aspects,
        bool optionSelectedByDefault = false)
        : base(name, description, "_OptionalSelect", aspects)
    {
        OptionDescription = optionDescription;
        SelectDescription = selectDescription;
        IsOptionSelected = optionSelectedByDefault;
        OptionSelectedName = $"{Name}{IFilterOptionalSelect<TFilteringResults>.OptionSelectedPostfix}";
    }

    public override IFilterOptionalSelect<TFilteringResults> Apply(IQueryCollection query)
    {
        return new AppliedFilterOptionalSelect<TFilteringResults>(this, query);
    }
}