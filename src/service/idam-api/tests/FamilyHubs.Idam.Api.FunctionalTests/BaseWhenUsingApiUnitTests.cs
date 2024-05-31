using FamilyHubs.Idam.Data.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

namespace FamilyHubs.Idam.Api.FunctionalTests;

#pragma warning disable S3881

public abstract class BaseWhenUsingApiUnitTests : IDisposable
{
    protected readonly HttpClient? Client;
    protected readonly CustomWebApplicationFactory? _webAppFactory;
    private readonly bool _initSuccessful;

    protected BaseWhenUsingApiUnitTests()
    {
        try
        {
            _initSuccessful = true;
            if (!IsRunningLocally())
            {
                _initSuccessful = false;
                return;
            }

            _initSuccessful = false;

            _webAppFactory = new CustomWebApplicationFactory();
            

            _webAppFactory.SetupTestData();

            Client = _webAppFactory.CreateDefaultClient();
            Client.BaseAddress = new Uri("https://localhost:7030/api/");

            _initSuccessful = true;

            

            
        }
        catch
        {
            _initSuccessful = false;
        }
    }

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

    /// <summary>
    /// Creates HttpRequestMessage
    /// </summary>
    /// <param name="role">If left blank request will not have bearer Token</param>
    public HttpRequestMessage CreatePostRequest(string path, object content, string role = "")
    {
        var request = CreateHttpRequestMessage(HttpMethod.Post, path, role);
        request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        return request;
    }

    /// <summary>
    /// Creates HttpRequestMessage
    /// </summary>
    /// <param name="role">If left blank request will not have bearer Token</param>
    public HttpRequestMessage CreatePutRequest(string path, object content, string role = "")
    {
        var request = CreateHttpRequestMessage(HttpMethod.Put, path, role);
        request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        return request;
    }

    /// <summary>
    /// Creates HttpRequestMessage
    /// </summary>
    /// <param name="role">If left blank request will not have bearer Token</param>
    public HttpRequestMessage CreateDeleteRequest(string path, object content, string role = "")
    {
        var request = CreateHttpRequestMessage(HttpMethod.Delete, path, role);
        request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        return request;
    }

    /// <summary>
    /// Creates HttpRequestMessage
    /// </summary>
    /// <param name="role">If left blank request will not have bearer Token</param>
    public HttpRequestMessage CreateGetRequest(string path, string role = "")
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

    
    protected bool IsRunningLocally()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        if (!_initSuccessful || configuration == null)
        {
            return false;
        }

        try
        {
            string localMachineName = configuration["LocalSettings:MachineName"] ?? string.Empty;

            if (!string.IsNullOrEmpty(localMachineName))
            {
                return Environment.MachineName.Equals(localMachineName, StringComparison.OrdinalIgnoreCase);
            }
        }
        catch
        {
            return false;
        }

        // Fallback to a default check if User Secrets file or machine name is not specified
        // For example, you can add additional checks or default behavior here
        return false;
    }

}

#pragma warning restore S3881