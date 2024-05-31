using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;
using FamilyHubs.SharedKernel.Razor.Header;
using FamilyHubs.SharedKernel.Razor.Links;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyHubs.Example.Pages.Examples.HeaderLinks.LinkModification;

public class AddingModel : PageModel, IFamilyHubsHeader
{
    IEnumerable<IFhRenderLink> IFamilyHubsHeader.NavigationLinks(
        FhLinkOptions[] navigationLinks,
        IFamilyHubsUiOptions familyHubsUiOptions)
    {
        return navigationLinks.Concat(navigationLinks.Where(l => l.Status != LinkStatus.Active));
    }

    IEnumerable<IFhRenderLink> IFamilyHubsHeader.ActionLinks(
        FhLinkOptions[] actionLinks,
        IFamilyHubsUiOptions familyHubsUiOptions)
    {
        return actionLinks.Concat(actionLinks.Where(l => l.Status != LinkStatus.Active));
    }
}