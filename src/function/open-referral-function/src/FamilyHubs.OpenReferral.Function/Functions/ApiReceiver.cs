using System.Net;
using FamilyHubs.OpenReferral.Function.ClientServices;
using FamilyHubs.OpenReferral.Function.Entities;
using FamilyHubs.OpenReferral.Function.Repository;
using FamilyHubs.ServiceDirectory.Data.Entities.Staging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;

namespace FamilyHubs.OpenReferral.Function.Functions;

public class ApiReceiver(
    ILogger<ApiReceiver> logger,
    HsdaApiService hsdaApiService,
    IFunctionDbContext functionDbContext)
{
    [Function("ApiReceiver")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "POST")] HttpRequestData req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        (HttpStatusCode httpStatusCode, List<ServiceJson>? serviceJsonList) = await hsdaApiService.GetServices();

        if (httpStatusCode != HttpStatusCode.OK) return req.CreateResponse(httpStatusCode);

        try
        {
            await UpdateDatabase(serviceJsonList);
        }
        catch (Exception e)
        {
            logger.LogError("{exception}", e.Message);
            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }

        return req.CreateResponse(HttpStatusCode.OK);
    }

    private async Task UpdateDatabase(List<ServiceJson>? serviceJsonList)
    {
        ArgumentNullException.ThrowIfNull(serviceJsonList);

        logger.LogInformation("Truncating database before inserting services..");
        await functionDbContext.TruncateServicesTempAsync();

        foreach (ServiceJson serviceJson in serviceJsonList)
        {
            logger.LogInformation("Adding service with ID {serviceId} to the database..", serviceJson.Id);
            functionDbContext.AddServiceTemp(new ServicesTemp
            {
                Id = Guid.Parse(serviceJson.Id),
                Json = serviceJson.Json,
                LastModified = DateTime.UtcNow
            });
        }

        logger.LogInformation("Saving changes to the database..");
        await functionDbContext.SaveChangesAsync();
    }
}