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

        Dictionary<string, string> serviceList = await GetServiceListFromApi();

        logger.LogInformation("Service Count -> {serviceCount}", serviceList.Count);

        await UpdateDatabase(serviceList);

        return new OkObjectResult("Welcome to Azure Functions!");
    }

    private static async Task<Dictionary<string, string>> GetServiceListFromApi()
    {
        using HttpResponseMessage response = await HttpClient.GetAsync("/services");
        string jsonResponse = await response.Content.ReadAsStringAsync();

        JArray serviceList = JArray.Parse(JObject.Parse(jsonResponse)["contents"]!.ToString());
        Dictionary<string, string> serviceByIdMap = new Dictionary<string, string>();

        foreach (JToken service in serviceList)
        {
            string serviceId = service["id"]!.ToString();
            serviceByIdMap.Add(serviceId, await GetServiceByIdFromApi(serviceId));
        }

        return serviceByIdMap;
    }

    private static async Task<string> GetServiceByIdFromApi(string serviceId)
    {
        using HttpResponseMessage response = await HttpClient.GetAsync("/services/" + serviceId);
        return await response.Content.ReadAsStringAsync();
    }

    private async Task UpdateDatabase(Dictionary<string, string> serviceByIdMap)
    {
        await functionDbContext.TruncateServicesTempAsync();

        foreach (KeyValuePair<string, string> service in serviceByIdMap)
        {
            functionDbContext.AddServiceTemp(new ServicesTemp
            {
                Id = Guid.Parse(service.Key),
                Json = service.Value,
                LastModified = DateTime.UtcNow
            });
        }

        await functionDbContext.SaveChangesAsync();
    }
}