
namespace FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;

public class FamilyHubsUiOptions : IFamilyHubsUiOptions
{
    public const string FamilyHubsUi = "FamilyHubsUi";

    public string ServiceName { get; set; } = "";
    public Phase Phase { get; set; }
    public string FeedbackUrl { get; set; } = "";

    /// <summary>
    /// The support email address for the service, as displayed on error pages.
    /// </summary>
    public string SupportEmail { get; set; } = "";

    /// <summary>
    /// If set, the path prefix will be prepended to all files included through the layout,
    /// e.g. css, js and asset files.
    /// Useful for when the site is being used behind an App Gateway using path based routing.
    /// </summary>
    public string PathPrefix { get; set; } = ""; 

    public Dictionary<string, string> Urls { get; set; } = new();

    public AnalyticsOptions? Analytics { get; set; }

    public HeaderOptions Header { get; set; } = new();
    public FooterOptions Footer { get; set; } = new();

    public Dictionary<string, FamilyHubsUiOptions> AlternativeFamilyHubsUi { get; set; } = new();

    /// <summary>
    /// Only relevant for alternative FamilyHubsUi options. If false, the alternative FamilyHubsUi options will not be used
    /// (even when specified through IAlternativeService).
    /// </summary>
    public bool Enabled { get; set; } = true;

    public string? AltName { get; private set; }
    public FamilyHubsUiOptions? Parent { get; private set; }

    public void SetAlternative(string? altName, FamilyHubsUiOptions? options)
    {
        AltName = altName;
        Parent = options;
    }

    public FamilyHubsUiOptions GetAlternative(string serviceName)
    {
        if (!AlternativeFamilyHubsUi.TryGetValue(serviceName, out var alternativeFamilyHubsUi))
        {
            throw new ArgumentException($"No alternative FamilyHubsUi options found for service \"{serviceName}\"", nameof(serviceName));
        }

        return alternativeFamilyHubsUi;
    }

    /// <summary>
    /// Returns an absolute URL from the Urls config section/dictionary, with an optional relativeUrl applied to the base..
    /// </summary>
    /// <typeparam name="TUrlKeyEnum">An enum where the value names match the keys in the Urls config section/dictionary.</typeparam>
    /// <param name="baseUrl">An enum value with the same name as a key under the Urls config section/dictionary.
    /// The corresponding value (from the config section/dictionary) is used as a base to the (optional) relative URL.
    /// The value is assumed to be an absolute url ending on with the host, port or path.
    /// If it ends with a path, it doesn't matter if it ends with a trailing slash or not.</param>
    /// <param name="relativeUrl">The relative URL (as applied to the baseUrl).
    /// Can contain a path and/or query parameters and/or a fragment.
    /// No URL encoding is applied to the relativeUrl.</param>
    /// <returns>The baseUrl with the relativeUrl applied (if supplied).</returns>
    /// <exception cref="ArgumentException">Thrown if the baseUrl is not a key in the Urls config section/dictionary.</exception>
    public Uri Url<TUrlKeyEnum>(TUrlKeyEnum baseUrl, string? relativeUrl = null)
        where TUrlKeyEnum : struct, Enum
    {
        return Url(baseUrl.ToString(), relativeUrl);
    }

    public Uri Url(string baseUrlKeyName, string? relativeUrl = null)
    {
        //todo: possibly cache from config as Uri's?
        if (!Urls.TryGetValue(baseUrlKeyName, out var baseUrlValue))
        {
            if (Parent != null)
            {
                return Parent.Url(baseUrlKeyName, relativeUrl);
            }
            //todo: if in alternative, add to exception message - will need to add alternative name to options
            throw new ArgumentException($"No path found in FamilyHubsUi Urls for key \"{baseUrlKeyName}\"", baseUrlKeyName);
        }

        return new Uri($"{baseUrlValue.TrimEnd('/')}/{relativeUrl?.TrimStart('/')}");
    }
}
