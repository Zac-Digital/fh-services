using FamilyHubs.SharedKernel.Razor.AlternativeServices;
using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace FamilyHubs.Example.Pages.Examples.Alternative;

public class IndexModel : PageModel, IAlternativeService
{
    public string ServiceName => "AlternativeService1";

    public string AlternativeServiceName { get; set; }

    public IndexModel(IOptions<FamilyHubsUiOptions> familyHubsUiOptions)
    {
        AlternativeServiceName = familyHubsUiOptions.Value.GetAlternative(ServiceName).ServiceName;
    }
}