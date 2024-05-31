using FamilyHubs.Example.Models;
using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;
using FamilyHubs.SharedKernel.Razor.Header;
using FamilyHubs.SharedKernel.Razor.Links;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyHubs.Example.Pages.Examples.HeaderLinks.LinkModification;

public class ReplaceModel : PageModel, IFamilyHubsHeader
{
    IEnumerable<IFhRenderLink> IFamilyHubsHeader.NavigationLinks(
        FhLinkOptions[] navigationLinks,
        IFamilyHubsUiOptions familyHubsUiOptions)
    {
        return navigationLinks.Select(x => new FhRenderLink(x)
        {
            Text = "Replaced",
            Url = familyHubsUiOptions.Url(UrlKey.ConnectWeb, x.Text).ToString()
        });
    }

    IEnumerable<IFhRenderLink> IFamilyHubsHeader.ActionLinks(
        FhLinkOptions[] actionLinks,
        IFamilyHubsUiOptions familyHubsUiOptions)
    {
        return new List<IFhRenderLink>
        {
            new FhRenderLink("New link") {Url = "https://example.com/new"}
        };
    }
}