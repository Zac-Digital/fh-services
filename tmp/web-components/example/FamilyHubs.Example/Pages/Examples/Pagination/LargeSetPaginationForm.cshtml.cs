using FamilyHubs.SharedKernel.Razor.Pagination;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyHubs.Example.Pages.Examples.Pagination;

public class LargeSetPaginationFormModel : PageModel
{
    public IPagination Pagination { get; set; } = IPagination.DontShow;

    public void OnGet()
    {
        Pagination = new LargeSetPagination(20, 1);
    }

    public void OnPost(int pageNum)
    {
        Pagination = new LargeSetPagination(20, pageNum);
    }
}