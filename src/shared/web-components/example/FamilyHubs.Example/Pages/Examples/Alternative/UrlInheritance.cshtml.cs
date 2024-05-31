using FamilyHubs.Example.Models;
using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace FamilyHubs.Example.Pages.Examples.Alternative
{
    public class UrlInheritanceModel : PageModel
    {
        public string ParentExampleUrl { get; set; }
        public string OverriddenConnectUrl { get; set; }

        public UrlInheritanceModel(IOptions<FamilyHubsUiOptions> familyHubsUiOptions)
        {
            var options = familyHubsUiOptions.Value;
            var alt = options.GetAlternative("UrlInheritance");

            OverriddenConnectUrl = alt.Url(UrlKey.ConnectWeb).ToString();
            ParentExampleUrl = alt.Url(UrlKey.ExampleWeb).ToString();
        }
    }
}
