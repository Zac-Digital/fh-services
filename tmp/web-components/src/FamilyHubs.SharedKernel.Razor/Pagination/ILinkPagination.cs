
namespace FamilyHubs.SharedKernel.Razor.Pagination;

public interface ILinkPagination : IPagination
{
    static new ILinkPagination DontShow => new DontShowLinkPagination();

    string GetUrl(int page);
}