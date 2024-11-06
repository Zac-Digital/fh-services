using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Api;
using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.Referral.FunctionalTests;

public class ServiceDirectoryFactory : WebApplicationFactory<Program>
{
    private readonly string _sdConnection = $"Data Source=sd-{Random.Shared.Next().ToString()}.db;Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;Recursive Triggers=True;Default Timeout=30;Pooling=True";

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // Get service provider.
        var serviceProvider = host.Services;

        // Create a scope to obtain a reference to the database
        // context (AppDbContext).
        using (var scope = serviceProvider.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();

            // Ensure the database is created.
            db.Database.EnsureCreated();
        }

        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var efCoreServices = services.Where(s =>
                s.ServiceType.FullName?.Contains("EntityFrameworkCore") == true).ToList();

            efCoreServices.ForEach(s => services.Remove(s));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(_sdConnection, mg =>
                    mg.UseNetTopologySuite().MigrationsAssembly(typeof(ApplicationDbContext).Assembly.ToString()));
            });
        });

        builder.UseEnvironment("Development");
    }

    public void SetupTestDatabaseAndSeedData()
    {
        using var scope = Services.CreateScope();

        var scopedServices = scope.ServiceProvider;
        var logger = scopedServices.GetRequiredService<ILogger<ServiceDirectoryFactory>>();

        try
        {
            var context = scopedServices.GetRequiredService<ApplicationDbContext>();

            if (!context.Services.Any())
                SeedServices(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the database with test messages. Error: {exceptionMessage}", ex.Message);
        }
    }

    private void SeedServices(ApplicationDbContext context)
    {
        var services = new List<Service>
        {
            new()
            {
                Id = 1,
                ServiceType =           ServiceType.InformationSharing,
                Name =                  "Elop Mentoring",
                Description =           "Elop Mentoring",
                OrganisationId =        1
            },
            new()
            {
                Id = 2,
                ServiceType =           ServiceType.InformationSharing,
                Name =                  "Collingwood Youth Centre",
                Description =           "Collingwood Youth Centre",
                OrganisationId =        2
            },
            new()
            {
                Id = 3,
                ServiceType =           ServiceType.InformationSharing,
                Name =                  "Newark Youth London",
                Description =           "Newark Youth London",
                OrganisationId =        3
            }
        };

        context.Services.AddRange(services);
        context.SaveChanges();
    }
}