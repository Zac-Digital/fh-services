
namespace FamilyHubs.SharedKernel.Razor.Filtering.Interfaces;

public interface IPageFilterFactory<TFilteringResults>
{
    Task<IEnumerable<TFilteringResults>> GetDefaultFilters();
}