using FamilyHubs.SharedKernel.Identity;
using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;
using FamilyHubs.SharedKernel.Razor.Header;
using FamilyHubs.SharedKernel.Razor.Links;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.FeatureManagement;

namespace FamilyHubs.Referral.Web.Pages.Shared;

public class HeaderPageModel : PageModel, IFamilyHubsHeader
{
    private readonly string? _selectedLinkText;
    
    public HeaderPageModel(bool highlightSearchForService = true, bool highlightRequests = false)
    {
        _selectedLinkText = highlightSearchForService switch
        {
            true => "Search for service",
            false when (highlightRequests) => "My requests",
            _ => null
        };
    }

    public bool ShowActionLinks => IsAuthenticatedAndTermsAccepted;
    public bool ShowNavigationMenu => IsAuthenticatedAndTermsAccepted;

    private bool IsAuthenticatedAndTermsAccepted =>
        User.Identity?.IsAuthenticated == true
        && HttpContext.TermsAndConditionsAccepted();

    LinkStatus IFamilyHubsHeader.GetStatus(IFhRenderLink link) =>
        link.Text == _selectedLinkText ? LinkStatus.Active : LinkStatus.Visible;

    IEnumerable<IFhRenderLink> IFamilyHubsHeader.NavigationLinks(
        FhLinkOptions[] navigationLinks,
        IFamilyHubsUiOptions familyHubsUiOptions)
    {
        // // Since 40-50 classes derive this class, using normal dependency injection would mean passing IFeatureManager
        // // into all their constructors as most of them call base(..) to pass through stuff to this constructor.
        // // So I'm fetching it in here :)
        // IFeatureManager featureManager = HttpContext.RequestServices.GetRequiredService<IFeatureManager>();
        //
        // if (! await featureManager.IsEnabledAsync(FeatureManagement.FeatureConnectDashboard))
        // {
        //     // The navigation links are loaded from the appsettings.json at startup for some reason so the easiest thing to do is
        //     // to just chop off the "My requests" tab from the array.
        //     return [ navigationLinks[0] ];
        // }
        
        string role = HttpContext.GetRole();

        return role is RoleTypes.VcsProfessional or RoleTypes.VcsDualRole
            ? familyHubsUiOptions.GetAlternative("VcsHeader").Header.NavigationLinks
            : navigationLinks;
    }
}