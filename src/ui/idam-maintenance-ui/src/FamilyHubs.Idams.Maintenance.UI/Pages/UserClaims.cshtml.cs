using Azure.Identity;
using FamilyHubs.Idams.Maintenance.Core.Services;
using FamilyHubs.Idams.Maintenance.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyHubs.Idams.Maintenance.UI.Pages;

public class UserClaimsModel : PageModel
{
    private readonly IIdamService _idamService;

    public string UserName { get; set; } = string.Empty;

    [BindProperty]
    public long AccountId { get; set; }

    public List<AccountClaim> UserClaims { get; private set; } = new List<AccountClaim>();

public UserClaimsModel(IIdamService idamService)
    {
       _idamService = idamService;
    }
    
    public async Task OnGet(long accountId, string username)
    {
        AccountId = accountId;
        UserName = username;

        UserClaims = await _idamService.GetAccountClaimsById(accountId);


    }

}
