using FamilyHubs.Idams.Maintenance.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyHubs.Idams.Maintenance.UI.Pages;

public class ModifiedUserClaimsConfirmationModel : PageModel
{
    private readonly IIdamService _idamService;

    public string Name { get; private set; } = string.Empty;

    public ModifiedUserClaimsConfirmationModel(IIdamService idamService)
    {
        _idamService = idamService;
    }
    public async Task OnGet(long accountId)
    {
        var account = await _idamService.GetAccountById(accountId);
        if (account != null)
        {
            Name = account.Name;
        }
    }
}
