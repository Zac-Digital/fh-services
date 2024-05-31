
namespace FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options.HealthCheck;

public class FhHealthCheckOptions
{
    public bool Enabled { get; set; }

    public Dictionary<string, HealthCheckUrlOptions> InternalApis { get; set; } = new();

    public Dictionary<string, HealthCheckUrlOptions> ExternalApis { get; set; } = new();

    public Dictionary<string, HealthCheckUrlOptions> ExternalSites { get; set; } = new();

    public Dictionary<string, HealthCheckDatabaseOptions> Databases { get; set; } = new();
}