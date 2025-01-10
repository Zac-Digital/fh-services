using FamilyHubs.ServiceDirectory.Admin.Core.DistributedCache;
using FamilyHubs.ServiceDirectory.Admin.Core.Models;
using FamilyHubs.ServiceDirectory.Admin.Core.Models.ServiceJourney;
using FamilyHubs.ServiceDirectory.Admin.Web.Journeys;
using FamilyHubs.ServiceDirectory.Admin.Web.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace FamilyHubs.ServiceDirectory.Admin.Web.Pages.manage_services;

public class Time_Details_At_LocationModel : ServicePageModel<TimeDetailsUserInput>
{
    public string? Title { get; set; }
    public int? MaxLength => 300;

    [BindProperty]
    public TimeDetailsUserInput UserInput { get; set; } = new();

    public Time_Details_At_LocationModel(IRequestDistributedCache connectionRequestCache)
        : base(ServiceJourneyPage.Time_Details_At_Location, connectionRequestCache)
    {
    }

    protected override void OnGetWithError()
    {
        UserInput = ServiceModel!.UserInput!;
        var location = ServiceModel!.CurrentLocation!;
        SetTitle(location);
    }

    protected override void OnGetWithModel()
    {
        var location = GetLocation();
        SetTitle(location);

        if (location.HasTimeDetails == true)
        {
            UserInput.HasDetails = true;
            UserInput.Description = location.TimeDescription;
        }
        else if (location.HasTimeDetails == false)
        {
            UserInput.HasDetails = false;
        }
    }

    private ServiceLocationModel GetLocation()
    {
        var locationIdString = Request.Query["locationId"].ToString();
        if (locationIdString == "") return ServiceModel!.CurrentLocation!;

        // user has asked to redo a specific location
        var locationId = long.Parse(locationIdString);

        return ServiceModel!.GetLocation(locationId);
    }

    private void SetTitle(ServiceLocationModel location)
    {
        Title = $"Can you provide more details about using this service at {location.DisplayName}?";

        var redo = Request.Query["redo"].ToString();
        if (!string.IsNullOrEmpty(redo))
        {
            BackUrl = GetServicePageUrl(ServiceJourneyPageExtensions.FromSlug(redo), ChangeFlow);
        }
    }

    protected override IActionResult OnPostWithModel()
    {
        var locationIdString = Request.Query["locationId"].ToString();
        var queryCollection = new Dictionary<string, StringValues>();
        if (locationIdString != "")
        {
            queryCollection.Add("locationId", locationIdString);
        }

        if (!UserInput.HasDetails.HasValue)
        {
            return RedirectToSelf(UserInput, queryCollection, ErrorId.Time_Details__MissingSelection);
        }

        if (UserInput.HasDetails == true && string.IsNullOrWhiteSpace(UserInput.Description))
        {
            return RedirectToSelf(UserInput, queryCollection, ErrorId.Time_Details_At_Location__MissingText);
        }

        if (UserInput.HasDetails == true && !string.IsNullOrWhiteSpace(UserInput.Description) && UserInput.Description.Replace("\r", "").Length > MaxLength)
        {
            return RedirectToSelf(UserInput, queryCollection, ErrorId.Time_Details_At_Location__DescriptionTooLong);
        }

        var location = GetLocation();

        ServiceModel!.Updated = true;

        if (UserInput.HasDetails == true)
        {
            location.HasTimeDetails = true;
            location.TimeDescription = UserInput.Description;
        }
        else
        {
            location.HasTimeDetails = false;
            location.TimeDescription = null;
        }

        var redo = Request.Query["redo"].ToString();
        if (redo != "")
        {
            return Redirect(GetServicePageUrl(ServiceJourneyPageExtensions.FromSlug(redo), ChangeFlow));
        }

        return NextPage();
    }
}