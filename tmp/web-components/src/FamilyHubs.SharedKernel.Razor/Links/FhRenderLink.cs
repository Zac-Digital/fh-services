using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;

namespace FamilyHubs.SharedKernel.Razor.Links;

//todo: add autodoc

public class FhRenderLink : IFhRenderLink
{
    public string Text { get; set; }
    public string? Url { get; set; }
    public bool OpenInNewTab { get; set; }
    public LinkStatus? Status { get; set; }

    public FhRenderLink(string text)
    {
        Text = text;
    }

    public FhRenderLink(FhLinkOptions fhLinkOptions)
    {
        Text = fhLinkOptions.Text;
        Url = fhLinkOptions.Url;
        OpenInNewTab = fhLinkOptions.OpenInNewTab;
        Status = fhLinkOptions.Status;
    }
}