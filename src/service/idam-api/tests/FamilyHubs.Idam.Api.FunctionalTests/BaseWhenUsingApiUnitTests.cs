using FamilyHubs.Idam.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace FamilyHubs.Idam.Api.FunctionalTests;

public abstract class BaseWhenUsingApiUnitTests : IDisposable
{
    protected readonly HttpClient? Client;
    private readonly CustomWebApplicationFactory? _webAppFactory;

    protected BaseWhenUsingApiUnitTests()
    {
        _webAppFactory = new CustomWebApplicationFactory(ConfigSetup);

        _webAppFactory.SetupTestData();

        Client = _webAppFactory.CreateDefaultClient();
        Client.BaseAddress = new Uri("https://localhost:7030/api/");
    }

    private static void ConfigSetup(IConfigurationBuilder builder) =>
        builder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "GovUkOidcConfiguration:BearerTokenSigningKey", "StubPrivateKey123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ" },
            { "Crypto:DbEncryptionKey", "188,7,221,249,250,101,147,86,47,246,21,252,145,56,161,150,195,184,64,43,55,0,196,200,98,220,95,186,225,8,224,75" },
            { "Crypto:DbEncryptionIVKey", "34,26,215,81,137,34,109,107,236,206,253,62,115,38,65,112" }
        });

    public void Dispose()
    {
        if (_webAppFactory != null)
        {
            using var scope = _webAppFactory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureDeleted();
        }
        
        if (Client != null) 
        {
            Client.Dispose();
        }
        
        if (_webAppFactory != null)
        {
            _webAppFactory!.Dispose();
        }
       
    }

    protected HttpRequestMessage CreatePostRequest(string path, object content, string role = "")
    {
        var request = CreateHttpRequestMessage(HttpMethod.Post, path, role);
        request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        return request;
    }

    protected HttpRequestMessage CreatePutRequest(string path, object content, string role = "")
    {
        var request = CreateHttpRequestMessage(HttpMethod.Put, path, role);
        request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        return request;
    }

    protected HttpRequestMessage CreateDeleteRequest(string path, object content, string role = "")
    {
        var request = CreateHttpRequestMessage(HttpMethod.Delete, path, role);
        request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        return request;
    }

    protected HttpRequestMessage CreateGetRequest(string path, string role = "")
    {
        var request = CreateHttpRequestMessage(HttpMethod.Get, path, role);
        return request;
    }

    private HttpRequestMessage CreateHttpRequestMessage(HttpMethod verb, string path, string role = "")
    {
        var request = new HttpRequestMessage
        {
            Method = verb,
            RequestUri = new Uri($"{Client!.BaseAddress}{path}"),
        };

        if (!string.IsNullOrEmpty(role))
        {
            request.Headers.Add("Authorization", $"Bearer {TestDataProvider.CreateBearerToken(role)}");
        }

        return request;
    }
}
