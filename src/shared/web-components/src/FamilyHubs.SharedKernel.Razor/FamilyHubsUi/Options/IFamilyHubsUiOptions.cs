namespace FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;

//todo: any value in this interface? can't easily inject it using IOptions<IFamilyHubsUiOptions>
public interface IFamilyHubsUiOptions
{
    /// <summary>
    /// The name of the service, as displayed in the header and elsewhere.
    /// </summary>
    string ServiceName { get; set; }

    Phase Phase { get; set; }
    string FeedbackUrl { get; set; }

    /// <summary>
    /// The support email address for the service, as displayed on error pages.
    /// </summary>
    string SupportEmail { get; set; }

    /// <summary>
    /// If set, the path prefix will be prepended to all files included through the layout,
    /// e.g. css, js and asset files.
    /// Useful for when the site is being used behind an App Gateway using path based routing.
    /// </summary>
    public string PathPrefix { get; set; }

    Dictionary<string, string> Urls { get; set; }
    AnalyticsOptions? Analytics { get; set; }
    HeaderOptions Header { get; set; }
    FooterOptions Footer { get; set; }

    FamilyHubsUiOptions GetAlternative(string serviceName);

    Uri Url<TUrlKeyEnum>(TUrlKeyEnum baseUrl, string? relativeUrl = null)
        where TUrlKeyEnum : struct, Enum;
}