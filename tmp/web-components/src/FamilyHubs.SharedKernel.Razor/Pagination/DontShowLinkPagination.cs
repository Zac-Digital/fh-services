
namespace FamilyHubs.SharedKernel.Razor.Pagination;

public class DontShowLinkPagination : DontShowPagination, ILinkPagination
{
    public string GetUrl(int page)
    {
        throw new NotImplementedException();
    }
}