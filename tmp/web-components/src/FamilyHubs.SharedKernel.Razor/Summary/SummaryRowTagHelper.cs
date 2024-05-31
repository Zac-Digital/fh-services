using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;

namespace FamilyHubs.SharedKernel.Razor.Summary;

public class SummaryRowTagHelper : TagHelper
{
    public string? Key { get; set; }
    public string? Action1 { get; set; }
    public string? Action1Href { get; set; }
    public string? Action2 { get; set; }
    public string? Action2Href { get; set; }
    public bool ShowEmpty { get; set; } = false;
    public string? Class { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(output);

        if (string.IsNullOrEmpty(Key))
        {
            throw new InvalidOperationException("The 'key' attribute is required.");
        }

        string finalValue = (await output.GetChildContentAsync()).GetContent();
        if (!string.IsNullOrWhiteSpace(finalValue) || ShowEmpty)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "govuk-summary-list__row");

            output.Content.SetHtmlContent(
                $@"<dt class='govuk-summary-list__key'>{Key}</dt>
                <dd class='govuk-summary-list__value {Class}'>{finalValue}</dd>");

            string divClass = "govuk-summary-list__row";
            if (string.IsNullOrWhiteSpace(Action1))
            {
                divClass += " govuk-summary-list__row--no-actions";
            }
            else
            {
                string action = ActionLink(Action1, Action1Href, Key);
                if (!string.IsNullOrWhiteSpace(Action2))
                {
                    action = $"<ul class='govuk-summary-list__actions-list'>{ActionListItem(action)}{ActionListItem(ActionLink(Action2, Action2Href, Key!))}</ul>";
                }
                output.Content.AppendHtml($"<dd class=\"govuk-summary-list__actions\">{action}</dd>");
            }
            output.Attributes.SetAttribute("class", divClass);
        }
        else
        {
            output.SuppressOutput();
        }
    }

    private string ActionLink(string action, string? href, string key)
    {
        return $"<a href='{href}'>{action}<span class='govuk-visually-hidden'> {key.ToLower(CultureInfo.CurrentCulture)}</span></a>";
    }

    private string ActionListItem(string actionLink)
    {
        return $"<li class='govuk-summary-list__actions-list-item'>{actionLink}</li>";
    }
}
