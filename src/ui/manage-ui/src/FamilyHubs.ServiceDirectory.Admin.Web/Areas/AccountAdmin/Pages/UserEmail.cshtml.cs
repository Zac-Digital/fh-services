using FamilyHubs.ServiceDirectory.Admin.Core.ApiClient;
using FamilyHubs.ServiceDirectory.Admin.Core.Helpers;
using FamilyHubs.ServiceDirectory.Admin.Core.Models;
using FamilyHubs.ServiceDirectory.Admin.Core.Services;
using FamilyHubs.ServiceDirectory.Admin.Web.Errors;
using FamilyHubs.ServiceDirectory.Admin.Web.ViewModel;
using FamilyHubs.SharedKernel.Razor.ErrorNext;
using FamilyHubs.SharedKernel.Razor.Header;
using Microsoft.AspNetCore.Mvc;

namespace FamilyHubs.ServiceDirectory.Admin.Web.Areas.AccountAdmin.Pages;

public class UserEmail : AccountAdminViewModel, IHasErrorStatePageModel
{
    private readonly IIdamClient _idamClient;

    public UserEmail(ICacheService cacheService, IIdamClient idamClient) : base(nameof(UserEmail), cacheService)
    {
        PageHeading = "What's their email address?";
        ErrorMessage = "Enter an email address";
        _idamClient = idamClient;
        Errors = ErrorState.Empty;
    }

    [BindProperty] 
    public required string EmailAddress { get; set; } = string.Empty;

    public IErrorState Errors { get; private set; }

    public override async Task OnGet()
    {
        await base.OnGet();
        
        EmailAddress = PermissionModel.EmailAddress;
    }
    
    public override async Task<IActionResult> OnPost()
    {
        await base.OnPost();

        if (ModelState.IsValid && ValidationHelper.IsValidEmail(EmailAddress))
        {
            if (await DoesEmailAlreadyExist())
            {
                await CacheService.StoreCurrentPageName($"/AccountAdmin/UserEmail/{CacheId}");
                return RedirectToPage("EmailAlreadyInUse");
            }

            PermissionModel.EmailAddress = EmailAddress;
            await CacheService.StorePermissionModel(PermissionModel, CacheId);
            
            return RedirectToPage(NextPageLink, new { cacheId = CacheId });
        }
        
        HasValidationError = true;
        Errors = ErrorState.Create(PossibleErrors.All, ErrorId.AccountAdmin_UserEmail);
        return Page();
    }

    private async Task<bool> DoesEmailAlreadyExist()
    {
        var account = await _idamClient.GetAccountBEmail(EmailAddress);
        if(account != null && account.Email == EmailAddress)
        {
            return true;
        }

        return false;
    }
}