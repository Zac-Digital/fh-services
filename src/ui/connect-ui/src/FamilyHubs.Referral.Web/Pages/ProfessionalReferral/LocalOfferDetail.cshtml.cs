using FamilyHubs.Referral.Core.ApiClients;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authorization;
using FamilyHubs.Referral.Web.Pages.Shared;
using FamilyHubs.SharedKernel.Identity;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.FeatureManagement;
using Microsoft.FeatureManagement;

namespace FamilyHubs.Referral.Web.Pages.ProfessionalReferral;

[Authorize(Roles = RoleGroups.LaOrVcsProfessionalOrDualRole)]
public class LocalOfferDetailModel : HeaderPageModel
{
    private readonly IOrganisationClientService _organisationClientService;
    private readonly IIdamsClient _idamsClient;
    private readonly IFeatureManager _featureManager;
    public ServiceDto LocalOffer { get; set; } = default!;
    public List<AttendingType>? ServiceScheduleAttendingTypes { get; set; }
    public ScheduleDto? ServiceSchedule { get; set; }

    public string? ReturnUrl { get; set; }

    [BindProperty]
    public string ServiceId { get; set; } = default!;

    public bool ShowConnectionRequestButton { get; set; }

    public LocalOfferDetailModel(
        IOrganisationClientService organisationClientService,
        IIdamsClient idamsClient,
        IFeatureManager featureManager)
    {
        _organisationClientService = organisationClientService;
        _idamsClient = idamsClient;
        _featureManager = featureManager;
    }

    public async Task<IActionResult> OnGetAsync(string serviceId)
    {
        ServiceId = serviceId;
        var referer = Request.Headers["Referer"];
        ReturnUrl = StringValues.IsNullOrEmpty(referer) ? Url.Page("Search") : referer.ToString();
        LocalOffer = await _organisationClientService.GetLocalOfferById(serviceId);

        (ServiceScheduleAttendingTypes, ServiceSchedule) = GetServiceSchedule();

        ShowConnectionRequestButton = await ShouldShowConnectionRequestButton();

        return Page();
    }

    // this only covers scenarios that can be created through Manage, not all possible scenarios from LA ingested data

    private (List<AttendingType>, ScheduleDto?) GetServiceSchedule()
    {
        var serviceScheduleAttendingTypes = new List<AttendingType>();

        foreach (var attendingTypeString in LocalOffer.Schedules
                     .Select(s => s.AttendingType))
        {
            if (attendingTypeString is not null
                && Enum.TryParse<AttendingType>(attendingTypeString, out var attendingType)
                && (attendingType != AttendingType.InPerson
                    || !LocalOffer.Locations.Any()))
            {
                serviceScheduleAttendingTypes.Add(attendingType);
            }
        }

        if (!serviceScheduleAttendingTypes.Any())
        {
            return (serviceScheduleAttendingTypes, null);
        }

        return (serviceScheduleAttendingTypes, LocalOffer.Schedules.FirstOrDefault());
    }

    private async Task<bool> ShouldShowConnectionRequestButton()
    {
        if (! await _featureManager.IsEnabledAsync(FeatureManagement.FeatureConnectDashboard))
        {
            return false;
        }
        
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
