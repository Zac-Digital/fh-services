using System.Diagnostics.Eventing.Reader;
using FamilyHubs.Referral.Core.ApiClients;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authorization;
using FamilyHubs.Referral.Web.Pages.Shared;
using FamilyHubs.SharedKernel.Identity;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.Referral.Web.Pages.ProfessionalReferral;

[Authorize(Roles = RoleGroups.LaOrVcsProfessionalOrDualRole)]
public class LocalOfferDetailModel : HeaderPageModel
{
    private readonly IOrganisationClientService _organisationClientService;
    private readonly IIdamsClient _idamsClient;
    public ServiceDto LocalOffer { get; set; } = default!;
    public ScheduleDto? ServiceSchedule { get; set; }

    public string? ReturnUrl { get; set; }

    [BindProperty]
    public string ServiceId { get; set; } = default!;

    public bool ShowConnectionRequestButton { get; set; }

    public LocalOfferDetailModel(
        IOrganisationClientService organisationClientService,
        IIdamsClient idamsClient)
    {
        _organisationClientService = organisationClientService;
        _idamsClient = idamsClient;
    }

    public async Task<IActionResult> OnGetAsync(string serviceId)
    {
        ServiceId = serviceId;
        var referer = Request.Headers["Referer"];
        ReturnUrl = StringValues.IsNullOrEmpty(referer) ? Url.Page("Search") : referer.ToString();
        LocalOffer = await _organisationClientService.GetLocalOfferById(serviceId);

        ServiceSchedule = GetServiceSchedule();

        ShowConnectionRequestButton = await ShouldShowConnectionRequestButton();

        return Page();
    }

    private ScheduleDto? GetServiceSchedule()
    {
        if (LocalOffer.Locations.Any())
            //|| LocalOffer.ServiceDeliveries?.All(sd => sd.Name != AttendingType.InPerson) == true)
        {
            return null;
        }
        return LocalOffer.Schedules
            .FirstOrDefault(s => s.AttendingType == AttendingType.InPerson.ToString());
    }

    private async Task<bool> ShouldShowConnectionRequestButton()
    {
        bool showConnectionRequestButton = HttpContext.GetRole() is
            RoleTypes.LaProfessional or RoleTypes.LaDualRole;
        if (showConnectionRequestButton)
        {
            var vcsProEmails = await _idamsClient
                .GetVcsProfessionalsEmailsAsync(LocalOffer.OrganisationId);
            showConnectionRequestButton = vcsProEmails.Any();
        }

        return showConnectionRequestButton;
    }
}
