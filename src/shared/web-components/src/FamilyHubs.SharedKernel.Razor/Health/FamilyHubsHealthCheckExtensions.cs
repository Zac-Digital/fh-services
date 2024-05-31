using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options.HealthCheck;
using FamilyHubs.SharedKernel.Razor.Health;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection;

public static class FamilyHubsHealthCheckExtensions
{
    public static IHealthChecksBuilder AddFamilyHubsHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration,
        string? appInsightsInstrumentationConfigKey = "APPINSIGHTS_INSTRUMENTATIONKEY")
    {
        // AddHealthChecks() is idempotent, so we don't have to worry about consumers calling it multiple times, or in any particular order
        var builder = services.AddHealthChecks();

        if (appInsightsInstrumentationConfigKey != null)
        {
            builder.AddAppInsights(configuration, appInsightsInstrumentationConfigKey);
        }

        var fhHealthCheckOptions = configuration.GetSection("FamilyHubsUi:HealthCheck").Get<FhHealthCheckOptions>();
        var urls = configuration.GetSection("FamilyHubsUi:Urls").Get<Dictionary<string, string>>();
        
        string? feedbackUrl = configuration["FamilyHubsUi:FeedbackUrl"];
        if (!string.IsNullOrEmpty(feedbackUrl))
        {
            fhHealthCheckOptions!.ExternalSites.Add("Feedback Site", new HealthCheckUrlOptions { Url = feedbackUrl });
        }

        var fhBuilder = new FhHealthChecksBuilder(builder, configuration, fhHealthCheckOptions, urls);

        fhBuilder.AddFamilyHubs();

        return builder;
    }

    private static IHealthChecksBuilder AddAppInsights(
        this IHealthChecksBuilder builder,
        IConfiguration config,
        string appInsightsInstrumentationConfigKey)
    {
        string? aiInstrumentationKey = config.GetValue<string>(appInsightsInstrumentationConfigKey);
        if (!string.IsNullOrEmpty(aiInstrumentationKey))
        {
            //todo: check in dev env
            builder.AddAzureApplicationInsights(aiInstrumentationKey, "App Insights", HealthStatus.Degraded, new[] { "Infrastructure" });
        }
        return builder;
    }

    public static WebApplication MapFamilyHubsHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}