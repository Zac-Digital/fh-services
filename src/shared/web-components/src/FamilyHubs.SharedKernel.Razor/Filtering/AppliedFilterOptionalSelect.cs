using FamilyHubs.SharedKernel.Razor.Filtering.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FamilyHubs.SharedKernel.Razor.Filtering;

public class AppliedFilterOptionalSelect<TFilteringResults>
    : AppliedFilter<TFilteringResults>, IFilterOptionalSelect<TFilteringResults>
{
    public bool IsOptionSelected { get; }
    public string OptionDescription => ((IFilterOptionalSelect<TFilteringResults>)Filter).OptionDescription;
    public string SelectDescription => ((IFilterOptionalSelect<TFilteringResults>)Filter).SelectDescription;
    public string OptionSelectedName { get; }

    public AppliedFilterOptionalSelect(IFilterOptionalSelect<TFilteringResults> filter, IQueryCollection query)
        : base(filter, query)
    {
        OptionSelectedName = $"{filter.Name}{IFilterOptionalSelect<TFilteringResults>.OptionSelectedPostfix}";

        var isOptionSelectedStr = query[OptionSelectedName];
        bool.TryParse(isOptionSelectedStr, out var isOptionSelected);
        IsOptionSelected = isOptionSelected;

        if (!IsOptionSelected)
        {
            SelectedAspects = Array.Empty<IFilterAspect>();
        }
    }
}