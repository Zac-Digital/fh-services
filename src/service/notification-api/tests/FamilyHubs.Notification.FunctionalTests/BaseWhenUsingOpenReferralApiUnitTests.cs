using AutoMapper;
using FamilyHubs.Notification.Api.Contracts;
using FamilyHubs.Notification.Core;
using FamilyHubs.Notification.Data.Entities;
using FamilyHubs.Notification.Data.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FamilyHubs.Notification.FunctionalTests;

public abstract class BaseWhenUsingOpenReferralApiUnitTests : IDisposable
{
    protected readonly HttpClient Client;
    private readonly CustomWebApplicationFactory? _webAppFactory;
    protected readonly JwtSecurityToken? Token;
    protected readonly string? EmailRecipient;
    protected readonly Dictionary<string, string>? Templates;

    protected BaseWhenUsingOpenReferralApiUnitTests()
    {
        var configBuilder = new ConfigurationBuilder();
        ConfigSetup(configBuilder);
        var conf = configBuilder.Build();

        EmailRecipient = conf.GetValue<string>("EmailRecipient") ?? string.Empty;

        string[] keys = { "ProfessionalAcceptRequest", "ProfessionalDecineRequest", "ProfessionalSentRequest", "VcsNewRequest" };

        Templates = new Dictionary<string, string>();
        foreach (string templatekey in keys)
        {
            var value = conf.GetValue<string>(templatekey);
            if (value != null)
                Templates[templatekey] = value;
        }

        var jti = Guid.NewGuid().ToString();
        var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(conf["GovUkOidcConfiguration:BearerTokenSigningKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        Token = new JwtSecurityToken(
            claims: new List<Claim>
            {
                new("sub", conf["GovUkOidcConfiguration:Oidc:ClientId"] ?? ""),
                new("jti", jti),
                new(ClaimTypes.Role, "Professional")
            },
            signingCredentials: creds,
            expires: DateTime.UtcNow.AddMinutes(5)
            );

        _webAppFactory = new CustomWebApplicationFactory(ConfigSetup);
        _webAppFactory.SetupTestDatabaseAndSeedData();

        Client = _webAppFactory.CreateDefaultClient();
        Client.BaseAddress = new Uri("https://localhost:7073/");
    }
    
    private static void ConfigSetup(IConfigurationBuilder builder) =>
        builder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "GovUkOidcConfiguration:BearerTokenSigningKey", "StubPrivateKey123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ" },
            { "Crypto:DbEncryptionKey", "188,7,221,249,250,101,147,86,47,246,21,252,145,56,161,150,195,184,64,43,55,0,196,200,98,220,95,186,225,8,224,75" },
            { "Crypto:DbEncryptionIVKey", "34,26,215,81,137,34,109,107,236,206,253,62,115,38,65,112" }
        });

    protected virtual void Dispose(bool disposing)
    {
        using var scope = _webAppFactory!.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureDeleted();

        Client.Dispose();
        _webAppFactory?.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected IMapper GetMapper()
    {
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        return mapper;
    }

    protected void SeedDatabase()
    {
        using var scope = _webAppFactory!.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.AddRange(GetNotificationList());
        context.SaveChanges();
    }

    protected List<SentNotification> GetNotificationList()
    {
        return new List<SentNotification>
        {
            new SentNotification
            {
                Id = 1,
                ApiKeyType = ApiKeyType.ManageKey,
                Notified = new List<Notified>
                {
                    new Notified
                    {
                        Id = 1,
                        NotificationId = 1,
                        Value = "Firstperson@email.com"
                    }
                },
                TemplateId = "11111",
                TokenValues = new List<TokenValue>
                {
                    new TokenValue
                    {
                        Id = 1,
                        NotificationId = 1,
                        Key = "Key1",
                        Value = "Value1"
                    }
                }

            },

            new SentNotification
            {
                Id = 2,
                ApiKeyType = ApiKeyType.ConnectKey,
                Notified = new List<Notified>
                {
                    new Notified
                    {
                        Id = 2,
                        NotificationId = 2,
                        Value = "Secondperson@email.com"
                    }
                },
                TemplateId = "2222",
                TokenValues = new List<TokenValue>
                {
                    new TokenValue
                    {
                        Id = 2,
                        NotificationId = 2,
                        Key = "Key2",
                        Value = "Value2"
                    }
                }

            },
        };
    }
}
