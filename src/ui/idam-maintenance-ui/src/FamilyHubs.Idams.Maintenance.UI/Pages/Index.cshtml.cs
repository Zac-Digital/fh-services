using FamilyHubs.SharedKernel.Identity;
using FamilyHubs.SharedKernel.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FamilyHubs.Idams.Maintenance.Core.Models;
using FamilyHubs.Idams.Maintenance.Core.Services;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.Idams.Maintenance.Core.ApiClient;
using FamilyHubs.Idams.Maintenance.UI.Pages.Shared;

namespace FamilyHubs.Idams.Maintenance.UI.Pages;

public class IndexModel : HeaderPageModel
{
    public IActionResult OnGet()
    {
        if (HttpContext.IsUserLoggedIn())
        {
            return RedirectToPage("/Welcome");
        }

        return Page();
    }
}