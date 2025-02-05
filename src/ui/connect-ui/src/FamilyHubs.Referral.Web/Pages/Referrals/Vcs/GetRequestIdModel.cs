using FamilyHubs.Referral.Web.Pages.Shared;
using FamilyHubs.SharedKernel.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FamilyHubs.Referral.Web.Pages.Referrals.Vcs;

[Authorize(Roles = $"{RoleGroups.VcsProfessionalOrDualRole}")]
public class GetRequestIdModel() : HeaderPageModel(false, true)
{
    public int? RequestId { get; private set; }

    public void OnGet(int id)
    {
        RequestId = id;
    }
}
