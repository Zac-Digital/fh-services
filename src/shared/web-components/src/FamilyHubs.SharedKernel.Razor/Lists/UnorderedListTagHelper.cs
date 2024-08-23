using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace FamilyHubs.SharedKernel.Razor.Lists;

[HtmlTargetElement("unordered-list")]
public class ListTagHelper : TagHelper
{
    /// <summary>
    /// An optional list of items to display in the list.
    /// </summary>
    //[HtmlAttributeName("items")]
    public IEnumerable<string>? Items { get; set; }

    //[HtmlAttributeName("bulleted")]
    public bool Bulleted { get; set; }// = false;

    //public override void Process(TagHelperContext context, TagHelperOutput output)
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";

        output.Attributes.SetAttribute("class", Bulleted ? "govuk-list govuk-list--bullet" : "govuk-list");

        string content;
        if (Items != null)
        {
            var sb = new StringBuilder();
            foreach (var item in Items)
            {
                sb.Append($"<li>{item}</li>");
            }

            content = sb.ToString();
        }
        else
        {
            content = (await output.GetChildContentAsync()).GetContent();
        }
        output.Content.SetHtmlContent(content);
    }
}