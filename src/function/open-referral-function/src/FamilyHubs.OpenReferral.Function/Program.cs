using FamilyHubs.OpenReferral.Function.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<IFunctionDbContext, FunctionDbContext>(options =>
        {
            options.UseSqlServer(Environment.GetEnvironmentVariable("ServiceDirectoryConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
    })
    .Build();

await host.RunAsync();