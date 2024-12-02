using FamilyHubs.ServiceDirectory.Admin.Core.Models;
using FamilyHubs.ServiceDirectory.Admin.Core.Services;
using FamilyHubs.ServiceDirectory.Admin.Web.Errors;
using FamilyHubs.ServiceDirectory.Admin.Web.ViewModel;
using FamilyHubs.SharedKernel.Razor.ErrorNext;
using FamilyHubs.SharedKernel.Razor.Header;
using Microsoft.AspNetCore.Mvc;

namespace FamilyHubs.ServiceDirectory.Admin.Web.Areas.AccountAdmin.Pages;

public class UserName : AccountAdminViewModel, IHasErrorStatePageModel
{
    public UserName(ICacheService cacheService) : base(nameof(UserName), cacheService)
    {
        PageHeading = "What's their full name?";
        ErrorMessage = "Enter a full name";
        Errors = ErrorState.Empty;
    }

    [BindProperty] 
    public required string FullName { get; set; } = string.Empty;

    public IErrorState Errors { get; private set; }

    public override async Task OnGet()
    {
        await base.OnGet();

        FullName = PermissionModel.FullName;
    }

    public override async Task<IActionResult> OnPost()
    {
        await base.OnPost();

        if (ModelState.IsValid && !string.IsNullOrWhiteSpace(FullName) && FullName.Length <= 255)
        {
            PermissionModel.FullName = FullName;
            await CacheService.StorePermissionModel(PermissionModel, CacheId);

            return RedirectToPage(NextPageLink, new { cacheId = CacheId});
        }

        HasValidationError = true;
        Errors = ErrorState.Create(PossibleErrors.All, ErrorId.AccountAdmin_UserName);
        return Page();
    }
}