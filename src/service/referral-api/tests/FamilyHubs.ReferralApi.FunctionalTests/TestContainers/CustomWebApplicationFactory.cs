using FamilyHubs.Referral.Api;
using FamilyHubs.Referral.Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.MsSql;

namespace FamilyHubs.Referral.FunctionalTests.TestContainers;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public readonly MsSqlContainer DbContainer = new MsSqlBuilder().Build();
    private readonly Action<IConfigurationBuilder> _configurationBuilder;

    public CustomWebApplicationFactory(Action<IConfigurationBuilder> configurationBuilder)
    {
        _configurationBuilder = configurationBuilder;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(_configurationBuilder);
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            List<ServiceDescriptor> efCoreServices = services.Where(s =>
                s.ServiceType.FullName?.Contains("EntityFrameworkCore") == true).ToList();

            efCoreServices.ForEach(s => services.Remove(s));
            
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(DbContainer.GetConnectionString(), mg => mg
                    .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.ToString())
                    .MigrationsHistoryTable("ReferralMigrationHistory"));
            });
        });
    }
}