using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;
using FamilyHubs.SharedKernel.Razor.Header;
using FamilyHubs.SharedKernel.Razor.Links;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyHubs.Example.Pages.Examples.HeaderLinks.Status;

/// <summary>
/// Status can be set during the NavigationLinks/ActionLinks methods.
/// Useful for when you're creating links, as it allows you to skip having to use GetStatus.
/// </summary>
public class LinksModel : PageModel, IFamilyHubsHeader
{
    IEnumerable<IFhRenderLink> IFamilyHubsHeader.NavigationLinks(
        FhLinkOptions[] navigationLinks,
        IFamilyHubsUiOptions familyHubsUiOptions)
    {
        return navigationLinks
            .Take(1)
            .Select(l => new FhRenderLink(l)
            {
                Text = "Not active home",
                Status = LinkStatus.Visible
            })
            .Concat<IFhRenderLink>(navigationLinks.Skip(1));
    }
}