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
    public List<AttendingType> ServiceScheduleAttendingTypes { get; set; } = default!;
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

        (ServiceScheduleAttendingTypes, ServiceSchedule) = GetServiceSchedule();

        ShowConnectionRequestButton = await ShouldShowConnectionRequestButton();

        return Page();
    }

    // this only covers scenarios that can be created through Manage, not all possible scenarios from LA ingested data

    //private (List<string>, IEnumerable<string>?) GetServiceSchedule()
    //private (List<string>, ScheduleDto?) GetServiceSchedule()
    private (List<AttendingType>, ScheduleDto?) GetServiceSchedule()
    {
        //var serviceScheduleAttendingTypeDescriptions = new List<string>();
        var serviceScheduleAttendingTypes = new List<AttendingType>();

        //var firstServiceSchedule = LocalOffer.Schedules.FirstOrDefault();

        //if (firstServiceSchedule == null)
        //{
        //    return (serviceScheduleAttendingTypeDescriptions, null);
        //}

        foreach (var attendingTypeString in LocalOffer.Schedules
                     .Select(s => s.AttendingType))
        {
            if (attendingTypeString != null
                && Enum.TryParse<AttendingType>(attendingTypeString, out var attendingType)
                && (attendingType != AttendingType.InPerson
                    || !LocalOffer.Locations.Any()))
            {
                //string desc = attendingType.ToDescription();
                //serviceScheduleAttendingTypeDescriptions.Add(desc[0] + desc[1..].ToLower());
                serviceScheduleAttendingTypes.Add(attendingType);
            }
        }

        if (!serviceScheduleAttendingTypes.Any())
        {
            return (serviceScheduleAttendingTypes, null);
        }

        return (serviceScheduleAttendingTypes, LocalOffer.Schedules.FirstOrDefault());

        //var firstServiceSchedule = LocalOffer.Schedules.FirstOrDefault();

        //return (serviceScheduleAttendingTypeDescriptions,
        //    firstServiceSchedule!.ByDay?.Split(",")
        //                       ?? Enumerable.Empty<string>()
        //        .Select(c => Calendar.DayCodeToName[c]));

        //return firstServiceSchedule;

        //if (LocalOffer.Locations.Any())
        //    //|| LocalOffer.ServiceDeliveries?.All(sd => sd.Name != AttendingType.InPerson) == true)
        //{
        //    return null;
        //}
        //return LocalOffer.Schedules
        //    .FirstOrDefault(s => s.AttendingType == AttendingType.InPerson.ToString());
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
