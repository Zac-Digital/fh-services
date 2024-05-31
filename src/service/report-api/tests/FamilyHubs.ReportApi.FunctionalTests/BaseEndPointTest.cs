using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using FamilyHubs.Report.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FamilyHubs.ReportApi.FunctionalTests;

public class BaseEndPointTest : IDisposable
{
    private bool _isInitialised;

    protected readonly HttpClient Client;
    private readonly CustomWebApplicationFactory _webApplicationFactory;

    protected const ServiceType ServiceTypeId = ServiceType.InformationSharing;

    private const string BearerTokenSigningKey = "StubPrivateKey123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    protected static readonly JsonSerializerOptions JsonOptions = new()
        { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    protected BaseEndPointTest()
    {
        _webApplicationFactory = new CustomWebApplicationFactory();

        Client = _webApplicationFactory.CreateDefaultClient();
        Client.BaseAddress = new Uri("https://localhost:7100/");
    }

    protected async Task InitialiseDatabase()
    {
        if (_isInitialised) return;

        await _webApplicationFactory.CreateAndSeedDatabase();

        _isInitialised = true;
    }

    protected HttpRequestMessage CreateHttpGetRequest(string url, string? role)
    {
        HttpRequestMessage request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{Client.BaseAddress}{url}"),
        };

        // If a role is required to access an endpoint, it needs the authorisation header added.
        if (!string.IsNullOrEmpty(role))
        {
            request.Headers.Add("Authorization", $"Bearer {CreateBearerToken(role)}");
        }

        return request;
    }

    private static string CreateBearerToken(string role)
    {
        List<Claim> claims = new List<Claim> { new("role", role) };
        ClaimsIdentity identity = new ClaimsIdentity(claims, "Test");
        ClaimsPrincipal user = new ClaimsPrincipal(identity);

        SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(BearerTokenSigningKey));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken token = new JwtSecurityToken(
            claims: user.Claims,
            signingCredentials: creds,
            expires: DateTime.UtcNow.AddMinutes(5)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public void Dispose()
    {
        if (!_isInitialised) return;

        using IServiceScope serviceScope = _webApplicationFactory.Services.CreateScope();
        ReportDbContext reportDbContext = serviceScope.ServiceProvider.GetRequiredService<ReportDbContext>();

        reportDbContext.Database.EnsureDeleted();

        Client.Dispose();
        _webApplicationFactory.Dispose();

        GC.SuppressFinalize(this);
    }
}
