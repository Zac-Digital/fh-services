using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace FamilyHubs.Example.Pages.contact_us;

public class IndexModel : PageModel
{
    public IFamilyHubsUiOptions FamilyHubsUiOptions { get; }

    public IndexModel(IOptions<FamilyHubsUiOptions> familyHubsUiOptions)
    {
        FamilyHubsUiOptions = familyHubsUiOptions.Value;
    }
}