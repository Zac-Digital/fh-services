using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FamilyHubs.SharedKernel.Razor.OpenCloseButton;

//todo: remove start-hidden from rendered html

/// <summary>
/// Adds a button that opens and closes the specified target element.
/// The button is only visible on mobile.
/// The user can show and hide the target on mobile,
/// but the target is always visible on tablet/desktop.
/// </summary>
[HtmlTargetElement("open-close-button")]
public class OpenCloseButtonTagHelper : TagHelper
{
    public string? Target { get; set; }
    public string? HideText { get; set; }

    public override async void Process(TagHelperContext context, TagHelperOutput output)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(output);

        if (string.IsNullOrEmpty(Target))
        {
            throw new InvalidOperationException("The 'Target' attribute is required.");
        }

        output.TagName = "button";

        string? existingClasses = output.Attributes.ContainsName("class")
            ? output.Attributes["class"].Value.ToString() : "";

        output.Attributes.SetAttribute("class", $"govuk-button govuk-button--secondary fh-open-close-button {existingClasses}");
        output.Attributes.SetAttribute("data-open-close-mobile", Target);
        if (HideText != null)
        {
            output.Attributes.SetAttribute("data-open-close-mobile-hide", HideText);
        }

        var startHiddenAttribute = context.AllAttributes.FirstOrDefault(a => a.Name.Equals("start-hidden", StringComparison.OrdinalIgnoreCase));
        bool startHidden =
            startHiddenAttribute != null
            && (startHiddenAttribute.ValueStyle == HtmlAttributeValueStyle.Minimized
                || bool.Parse((string)startHiddenAttribute.Value));

        if (startHidden)
        {
            output.Attributes.SetAttribute("data-open-close-mobile-default", "hide");
        }
        output.Attributes.SetAttribute("data-module", "govuk-button");
        output.Attributes.SetAttribute("type", "button");

        var childContent = await output.GetChildContentAsync();
        output.Content.SetContent(childContent.GetContent());
    }
}
