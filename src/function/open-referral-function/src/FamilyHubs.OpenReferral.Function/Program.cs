using FamilyHubs.OpenReferral.Function.ClientServices;
using FamilyHubs.OpenReferral.Function.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();
IHostEnvironment env = builder.Environment;

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
builder.Configuration.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);

IHost host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        var config = services.BuildServiceProvider().GetService<IConfiguration>();

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddSingleton<IHsdaApiService, HsdaApiService>();

        services.AddDbContext<IFunctionDbContext, FunctionDbContext>(options =>
        {
            options.UseSqlServer(Environment.GetEnvironmentVariable("ServiceDirectoryConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
    })
    .Build();

await host.RunAsync();

// TODO: AppSettings Loading
// TODO: Unit Testing