using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FamilyHubs.Referral.Data.Entities;
using FamilyHubs.Referral.Data.Repository;
using FamilyHubs.SharedKernel.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace FamilyHubs.Referral.FunctionalTests.TestContainers;

public class BaseWhenUsingReferralApiUnitTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _webApplicationFactory;
    protected readonly HttpClient HttpClient;
    
    protected readonly JwtSecurityToken? Token;
    protected readonly JwtSecurityToken? TokenLaManager;
    protected readonly JwtSecurityToken? TokenForOrganisationOne;
    protected readonly JwtSecurityToken? VcsToken;
    protected readonly JwtSecurityToken? ForbiddenToken;

    protected BaseWhenUsingReferralApiUnitTests()
    {
        ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        ConfigSetup(configurationBuilder);
        IConfigurationRoot configuration = configurationBuilder.Build();
        
        string jti = Guid.NewGuid().ToString();
        SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configuration["GovUkOidcConfiguration:BearerTokenSigningKey"]!));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        
        Token = new JwtSecurityToken(
            claims: new List<Claim>
            {
                new("sub", configuration["GovUkOidcConfiguration:Oidc:ClientId"] ?? ""),
                new("jti", jti),
                new(ClaimTypes.Role, RoleTypes.LaProfessional),
                new(FamilyHubsClaimTypes.OrganisationId, "1"),
                new(FamilyHubsClaimTypes.AccountId, "1")
            },
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddMinutes(5)
        );

        TokenLaManager = new JwtSecurityToken(
            claims: new List<Claim>
            {
                new("sub", configuration["GovUkOidcConfiguration:Oidc:ClientId"] ?? ""),
                new("jti", jti),
                new(ClaimTypes.Role, RoleTypes.LaManager),
                new(FamilyHubsClaimTypes.OrganisationId, "1"),
                new(FamilyHubsClaimTypes.AccountId, "1")
            },
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddMinutes(5)
        );

        TokenForOrganisationOne = new JwtSecurityToken(
            claims: new List<Claim>
            {
                new("sub", configuration["GovUkOidcConfiguration:Oidc:ClientId"] ?? ""),
                new("jti", jti),
                new("AccountId", "5"),
                new(ClaimTypes.Role, RoleTypes.LaProfessional),
                new(FamilyHubsClaimTypes.OrganisationId, "1"),
                new(FamilyHubsClaimTypes.AccountId, "1")
            },
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddMinutes(5)
        );

        VcsToken = new JwtSecurityToken(
            claims: new List<Claim>
            {
                new("sub", configuration["GovUkOidcConfiguration:Oidc:ClientId"] ?? ""),
                new("jti", jti),
                new(ClaimTypes.Role, RoleTypes.VcsProfessional),
                new(FamilyHubsClaimTypes.OrganisationId, "1"),
                new(FamilyHubsClaimTypes.AccountId, "1")
            },
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddMinutes(5)
        );

        ForbiddenToken = new JwtSecurityToken(
            claims: new List<Claim>
            {
                new("sub", configuration["GovUkOidcConfiguration:Oidc:ClientId"] ?? ""),
                new("jti", jti),
                new(ClaimTypes.Role, RoleTypes.VcsProfessional),
                new(FamilyHubsClaimTypes.OrganisationId, "-1"),
                new(FamilyHubsClaimTypes.AccountId, "-1")
            },
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddMinutes(5)
        );
        
        _webApplicationFactory = new CustomWebApplicationFactory(ConfigSetup);
        HttpClient = _webApplicationFactory.CreateDefaultClient();
        HttpClient.BaseAddress = new Uri("https://localhost:7192/");
    }
    
    public async Task InitializeAsync()
    {
        await _webApplicationFactory.DbContainer.StartAsync();
        using IServiceScope scope = _webApplicationFactory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        SetupTestDatabaseAndSeedData();
    }

    public async Task DisposeAsync()
    {
        await _webApplicationFactory.DbContainer.StopAsync();
        await _webApplicationFactory.DisposeAsync();
        HttpClient.Dispose();
    }
    
    private static void ConfigSetup(IConfigurationBuilder builder) => builder.AddInMemoryCollection(new Dictionary<string, string?> 
    { 
        { "GovUkOidcConfiguration:BearerTokenSigningKey", "StubPrivateKey123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ" },
        { "Crypto:DbEncryptionKey", "188,7,221,249,250,101,147,86,47,246,21,252,145,56,161,150,195,184,64,43,55,0,196,200,98,220,95,186,225,8,224,75" },
        { "Crypto:DbEncryptionIVKey", "34,26,215,81,137,34,109,107,236,206,253,62,115,38,65,112" }
    });

    private void SetupTestDatabaseAndSeedData()
    {
        using IServiceScope scope = _webApplicationFactory.Services.CreateScope();

        IServiceProvider scopedServices = scope.ServiceProvider;
        ILogger<CustomWebApplicationFactory> logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

        try
        {
            ApplicationDbContext context = scopedServices.GetRequiredService<ApplicationDbContext>();
            IReadOnlyCollection<Status> statuses = ReferralSeedData.SeedStatuses();

            if (!context.Statuses.Any())
            {
                context.Statuses.AddRange(statuses);
                context.SaveChanges();
            }

            if (context.Referrals.Any()) return;
            
            IReadOnlyCollection<Data.Entities.Referral> referrals = ReferralSeedData.SeedReferral();

            foreach (Data.Entities.Referral referral in referrals)
            {
                Status? status = context.Statuses.SingleOrDefault(x => x.Name == referral.Status.Name);
                if (status != null)
                {
                    referral.Status = status;
                }
            }

            context.Referrals.AddRange(referrals);
            context.SaveChanges();

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the database with test messages. Error: {exceptionMessage}", ex.Message);
        }
    }
}