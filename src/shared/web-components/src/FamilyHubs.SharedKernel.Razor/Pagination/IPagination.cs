
namespace FamilyHubs.SharedKernel.Razor.Pagination;

public interface IPagination
{
    static IPagination DontShow => new DontShowPagination();

    IEnumerable<PaginationItem> PaginationItems { get; }
    bool Show { get; }
    int? TotalPages { get; }
    int? CurrentPage { get; }
    int? PreviousPage { get; }
    int? NextPage { get; }
}