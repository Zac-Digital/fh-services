namespace FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options.Configure;

public class FamilyHubsUiOptionsValidation : IValidateOptions<FamilyHubsUiOptions>
{
#pragma warning disable S927 // parameter name _ doesn't match base, but indicates that we don't care about the value
    public ValidateOptionsResult Validate(string? _, FamilyHubsUiOptions options)
#pragma warning restore S927
    {
        var validationErrors = new List<string>();

        ValidateLinks(options, validationErrors);

        if (validationErrors.Any())
        {
            return ValidateOptionsResult.Fail(validationErrors);
        }
        return ValidateOptionsResult.Success;
    }

    private static void ValidateLinks(FamilyHubsUiOptions options, List<string> validationErrors)
    {
        ValidateLinks(options.Header.NavigationLinks, validationErrors, "Header navigation");
        ValidateLinks(options.Header.ActionLinks, validationErrors, "Header action");
        ValidateLinks(options.Footer.Links, validationErrors, "Footer");

        var enabledAlts = options.AlternativeFamilyHubsUi
            .Where(kvp => kvp.Value.Enabled)
            .Select(kvp => kvp.Value);

        // turtles all the way down
        foreach (var alt in enabledAlts)
        {
            ValidateLinks(alt, validationErrors);
        }
    }

    private static void ValidateLinks(FhLinkOptions[] linkOptions, List<string> validationErrors, string linkTypeDescription)
    {
        validationErrors.AddRange(
            linkOptions.Where(l => !Uri.IsWellFormedUriString(l.Url, UriKind.RelativeOrAbsolute))
            .Select(l => $"{linkTypeDescription} link for \"{l.Text}\" has invalid Url \"{l.Url}\""));
    }
}