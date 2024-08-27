using FamilyHubs.OpenReferral.Function.ClientServices;
using FamilyHubs.OpenReferral.Function.Entities;
using FamilyHubs.OpenReferral.Function.Repository;
using FamilyHubs.ServiceDirectory.Data.Entities.Staging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyHubs.OpenReferral.Function.Functions;

public class ApiReceiver(ILogger<ApiReceiver> logger, HsdaApiService hsdaApiService, IFunctionDbContext functionDbContext)
{
    [Function("ApiReceiver")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "POST")] HttpRequest request)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        ServiceJson[] serviceJsonList = await hsdaApiService.GetServices();

        await UpdateDatabase(serviceJsonList);

        return new OkObjectResult("Welcome to Azure Functions!");
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