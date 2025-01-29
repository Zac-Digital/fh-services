using FamilyHubs.SharedKernel.Razor.FeatureFlags;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

namespace FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options.Configure;

public class FamilyHubsUiOptionsConfigure : IConfigureOptions<FamilyHubsUiOptions>
{
    private readonly IConfiguration _configuration;
    private readonly IFeatureManager _featureManager;

    public FamilyHubsUiOptionsConfigure(IConfiguration configuration, IFeatureManager featureManager)
    {
        _configuration = configuration;
        _featureManager = featureManager;
    }

    public void Configure(FamilyHubsUiOptions options)
    {
        Configure(options, null, null);
    }

    private void Configure(FamilyHubsUiOptions options, string? altName, FamilyHubsUiOptions? parent)
    {
        options.SetAlternative(altName, parent);

        ConfigureLink(options.Header.ServiceNameLink, options);
        ConfigureNavigationLinks(options.Header.NavigationLinks, options);
        ConfigureLinks(options.Header.ActionLinks, options);
        ConfigureLinks(options.Footer.Links, options);

        var enabledAlts = options.AlternativeFamilyHubsUi
            .Where(kvp => kvp.Value.Enabled)
            .Select(kvp => kvp);

        // Recursively generates header permutations for each section defined in "AlternativeFamilyHubsUi" ..
        // .. in appsettings.json
        foreach (var alt in enabledAlts)
        {
            Configure(alt.Value, alt.Key, options);
        }
    }

    private void ConfigureNavigationLinks(FhLinkOptions[] linkOptions, FamilyHubsUiOptions options)
    {
        ConfigureLinks(linkOptions, options);

        IEnumerable<string> featureFlags = _featureManager.GetFeatureNamesAsync().ToBlockingEnumerable();

        if (!featureFlags.Contains(FeatureFlag.ConnectDashboard))
        {
            return;
        }
        
        if (!_featureManager.IsEnabledAsync(FeatureFlag.ConnectDashboard).Result)
        {
            options.Header.NavigationLinks = [ options.Header.NavigationLinks[0] ];
        }
    }

    private void ConfigureLinks(FhLinkOptions[] linkOptions, FamilyHubsUiOptions options)
    {
        foreach (var link in linkOptions)
        {
            ConfigureLink(link, options);
        }
    }

    private void ConfigureLink(FhLinkOptions link, FamilyHubsUiOptions options)
    {
        if (link.ConfigUrl != null)
        {
            link.Url = _configuration[link.ConfigUrl];
        }
        else
        {
            // if Url is not set, use a simple slugified version of the link text
            link.Url ??= $"/{link.Text.ToLowerInvariant().Replace(' ', '-')}";

            // if a base url key is set, treat the Url as a relative url from the given base
            if (!string.IsNullOrEmpty(link.BaseUrlKey))
            {
                //todo: catch and rethrow with more context? i.e. link trying to create
                link.Url = options.Url(link.BaseUrlKey, link.Url).ToString();
            }
        }
    }
}