using FamilyHubs.Example.Models;
using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace FamilyHubs.Example.Pages.Examples.Urls;

public class IndexModel : PageModel
{
    public Uri ExampleUrl { get; set; }

    public IndexModel(IOptions<FamilyHubsUiOptions> familyHubsUiOptions)
    {
        ExampleUrl = familyHubsUiOptions.Value.Url(UrlKey.ManageWeb, "example");
    }
}