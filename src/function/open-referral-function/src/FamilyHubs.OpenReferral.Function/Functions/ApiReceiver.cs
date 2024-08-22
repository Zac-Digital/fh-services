using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyHubs.OpenReferral.Function.Functions;

public class ApiReceiver
{
    private readonly ILogger<ApiReceiver> _logger;

    private readonly string? _connectionApi;
    private readonly string? _connectionDatabase;

    public ApiReceiver(ILogger<ApiReceiver> logger)
    {
        _logger = logger;
        _connectionApi = Environment.GetEnvironmentVariable("ConnectionApi");
        _connectionDatabase = Environment.GetEnvironmentVariable("ServiceDirectoryConnection");
    }

    [Function("ApiReceiver")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "POST")] HttpRequest request)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        HttpClient httpClient = new();
        using HttpResponseMessage response = await httpClient.GetAsync(_connectionApi + "/services");
        string jsonResponse = await response.Content.ReadAsStringAsync();

        _logger.LogInformation("{JsonResponse}", jsonResponse);

        return new OkObjectResult("Welcome to Azure Functions!");
    }

}