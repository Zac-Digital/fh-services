using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;
using FamilyHubs.SharedKernel.Razor.Links;

namespace FamilyHubs.SharedKernel.Razor.Header;

public interface IFamilyHubsHeader
{
    bool ShowNavigationMenu => true;
    //todo: default to something like this? (via HttpContext)... User.Identity?.IsAuthenticated == true;
    bool ShowActionLinks => true;

    LinkStatus GetStatus(IFhRenderLink link) => link.Status ?? LinkStatus.Visible;

    IEnumerable<IFhRenderLink> NavigationLinks(FhLinkOptions[] navigationLinks, IFamilyHubsUiOptions familyHubsUiOptions)
        => navigationLinks;

    IEnumerable<IFhRenderLink> ActionLinks(FhLinkOptions[] actionLinks, IFamilyHubsUiOptions familyHubsUiOptions)
        => actionLinks;
}