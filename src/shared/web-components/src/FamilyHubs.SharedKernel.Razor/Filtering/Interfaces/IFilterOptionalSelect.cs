
namespace FamilyHubs.SharedKernel.Razor.Filtering.Interfaces;

public interface IFilterOptionalSelect : IFilter
{
    const string OptionSelectedPostfix = "-option-selected";

    bool IsOptionSelected { get; }
    string OptionDescription { get; }
    string SelectDescription { get; }
    string OptionSelectedName { get; }
}

public interface IFilterOptionalSelect<in TFilteringResults> : IFilterOptionalSelect, IFilter<TFilteringResults>
{
}
