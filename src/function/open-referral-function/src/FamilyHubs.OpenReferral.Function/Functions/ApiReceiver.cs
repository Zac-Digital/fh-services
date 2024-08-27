using FamilyHubs.OpenReferral.Function.Entities;
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

        ServiceJson[] serviceJsonList = await GetServiceListFromApi();

        logger.LogInformation("Service Count -> {serviceCount}", serviceJsonList.Length);

        await UpdateDatabase(serviceJsonList);

        return new OkObjectResult("Welcome to Azure Functions!");
    }

    private static async Task<ServiceJson[]> GetServiceListFromApi()
    {
        using HttpResponseMessage response = await HttpClient.GetAsync("/services");
        string jsonResponse = await response.Content.ReadAsStringAsync();

        JArray serviceList = JArray.Parse(JObject.Parse(jsonResponse)["contents"]!.ToString());
        ServiceJson[] serviceJsonList = new ServiceJson[serviceList.Count];

        for (int i = 0; i < serviceList.Count; i++)
        {
            string serviceId = serviceList[i]["id"]!.ToString();
            serviceJsonList[i] = new ServiceJson()
            {
                Id = serviceId,
                Json = await GetServiceByIdFromApi(serviceId)
            };
        }

        return serviceJsonList;
    }

    private static async Task<string> GetServiceByIdFromApi(string serviceId)
    {
        using HttpResponseMessage response = await HttpClient.GetAsync("/services/" + serviceId);
        return await response.Content.ReadAsStringAsync();
    }

    private async Task UpdateDatabase(ServiceJson[] serviceJsonList)
    {
        await functionDbContext.TruncateServicesTempAsync();

        foreach (ServiceJson serviceJson in serviceJsonList)
        {
            functionDbContext.AddServiceTemp(new ServicesTemp
            {
                Id = Guid.Parse(serviceJson.Id),
                Json = serviceJson.Json,
                LastModified = DateTime.UtcNow
            });
        }

        await functionDbContext.SaveChangesAsync();
    }
}