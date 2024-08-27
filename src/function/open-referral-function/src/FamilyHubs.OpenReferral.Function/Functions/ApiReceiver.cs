using FamilyHubs.OpenReferral.Function.Repository;
using FamilyHubs.ServiceDirectory.Data.Entities.Staging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FamilyHubs.OpenReferral.Function.Functions;

public class ApiReceiver(ILogger<ApiReceiver> logger, IFunctionDbContext functionDbContext)
{
    private static readonly HttpClient HttpClient = new() { BaseAddress = new Uri(Environment.GetEnvironmentVariable("ConnectionApi")!) };

    [Function("ApiReceiver")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "POST")] HttpRequest request)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        string jsonResponse = await GetServicesFromApi();

        logger.LogInformation("{JsonResponse}", jsonResponse);

        await UpdateDatabase(jsonResponse);

        return new OkObjectResult("Welcome to Azure Functions!");
    }

    private async Task<string> GetServicesFromApi()
    {
        using HttpResponseMessage response = await HttpClient.GetAsync("/services");
        return await response.Content.ReadAsStringAsync();
    }

    private async Task UpdateDatabase(string jsonResponse)
    {
        await functionDbContext.TruncateServicesTempAsync();

        string serviceId = JObject.Parse(jsonResponse)["contents"]?[0]?["id"]?.ToString()!;

        functionDbContext.AddServiceTemp(new ServicesTemp
        {
            Id = Guid.Parse(serviceId),
            Json = jsonResponse,
            LastModified = DateTime.UtcNow
        });

        await functionDbContext.SaveChangesAsync();
    }
}