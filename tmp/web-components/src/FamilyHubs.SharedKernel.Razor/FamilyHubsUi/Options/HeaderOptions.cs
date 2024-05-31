
namespace FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;

public class HeaderOptions
{
    public FhLinkOptions ServiceNameLink { get; set; } = new() { Url = "/" };
    public FhLinkOptions[] NavigationLinks { get; set; } = Array.Empty<FhLinkOptions>();
    public FhLinkOptions[] ActionLinks { get; set; } = Array.Empty<FhLinkOptions>();
}