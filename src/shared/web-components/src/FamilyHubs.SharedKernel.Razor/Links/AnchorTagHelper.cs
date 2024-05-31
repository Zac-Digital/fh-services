using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FamilyHubs.SharedKernel.Razor.Links;

[HtmlTargetElement("a", Attributes = "web-page")]
[HtmlTargetElement("a", Attributes = "new-tab")]
[HtmlTargetElement("a", Attributes = "email")]
[HtmlTargetElement("a", Attributes = "phone")]
public class AnchorTagHelper : TagHelper
{
    public string? WebPage { get; set; }
    public bool NewTab { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrEmpty(WebPage))
        {
            output.Attributes.SetAttribute("href", WebPage);
            var childContent = await output.GetChildContentAsync();
            output.Content.SetContent(string.IsNullOrEmpty(childContent.GetContent()) ? WebPage : childContent.GetContent());
        }
        else if (!string.IsNullOrEmpty(Email))
        {
            output.Attributes.SetAttribute("href", $"mailto:{Email}");
            output.Content.SetContent(Email);
        }
        else if (!string.IsNullOrEmpty(Phone))
        {
            //todo: 2 separate fields? should be global number(+), but do we have that, should we construct it. is 'remove spaces' enough?
            // see https://www.rfc-editor.org/rfc/rfc3966#page-6

            output.Attributes.SetAttribute("href", $"tel:{Phone.Replace(" ", "")}");
            output.Content.SetContent(Phone);
        }

        if (NewTab)
        {
            output.Attributes.SetAttribute("target", "_blank");
            output.Attributes.SetAttribute("rel", "noopener noreferrer");
            output.PostContent.SetHtmlContent(" (opens in new tab)");
        }
    }
}