using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Azure.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.SharedKernel.DataProtection;

//https://stackoverflow.com/questions/72010688/asp-net-core-3-1-unable-to-unprotect-the-message-state-running-in-debugger
/// <remarks>
/// https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-7.0
/// https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/implementation/key-storage-providers?view=aspnetcore-7.0&tabs=visual-studio#entity-framework-core
/// </remarks>
public static class DataProtectionExtension
{
    public static void AddFamilyHubsDataProtection(this IServiceCollection services, IConfiguration configuration, string appName)
    {
        //todo: put the MigrationsHistoryTable and the DataProtectionKeys table in a "sharedkernel" schema
        const string schemaName = "dbo";

        // Add a DbContext to store your Database Keys
        services.AddDbContext<DataProtectionKeysContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("SharedKernelConnection"),
                ob =>
                {
                    ob.MigrationsHistoryTable("SharedKernelMigrationsHistory", schemaName)
                        .MigrationsAssembly(typeof(DataProtectionKeysContext).Assembly.ToString());
                }));

        var dataProtectionBuilder = services
            .AddDataProtection()
            .SetApplicationName(appName)
            .PersistKeysToDbContext<DataProtectionKeysContext>();

        AppendKeyVaultPersist(configuration, dataProtectionBuilder);
    }

    private static void AppendKeyVaultPersist(IConfiguration configuration, IDataProtectionBuilder dataProtectionBuilder)
    {
        var keyIdentifier = configuration["DataProtection:KeyIdentifier"];
        
        if (string.IsNullOrWhiteSpace(keyIdentifier)) return;

        if (!Uri.TryCreate(keyIdentifier, UriKind.Absolute, out var uri)) 
            throw new ArgumentException($"Invalid key identifier URI format: '{keyIdentifier}'");
        
        dataProtectionBuilder.ProtectKeysWithAzureKeyVault(uri, new ManagedIdentityCredential());
    }
}