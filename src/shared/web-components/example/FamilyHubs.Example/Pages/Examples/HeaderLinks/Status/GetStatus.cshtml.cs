using FamilyHubs.SharedKernel.Razor.Header;
using FamilyHubs.SharedKernel.Razor.Links;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyHubs.Example.Pages.Examples.HeaderLinks.Status;

/// <summary>
/// GetStatus is useful to be able to set the status of the links,
/// without having to create new IFhRenderLink implementing objects in NavigationLinks/ActionLinks.
/// </summary>
public class GetStatusModel : PageModel, IFamilyHubsHeader
{
    LinkStatus IFamilyHubsHeader.GetStatus(IFhRenderLink link)
    {
        return link.Text switch
        {
            "Request support" => LinkStatus.Active,
            "Don't show" => LinkStatus.NotVisible,
            _ => LinkStatus.Visible
        };
    }
}