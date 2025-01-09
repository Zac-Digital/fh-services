using System.Net;
using System.Text.Json;
using FamilyHubs.OpenReferral.Function.ClientServices;
using FamilyHubs.OpenReferral.Function.Repository;
using FamilyHubs.SharedKernel.OpenReferral.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;

namespace FamilyHubs.OpenReferral.Function.Functions;

public class TriggerPullServicesWebhook(
    ILogger<TriggerPullServicesWebhook> logger,
    IHsdaApiService hsdaApiService,
    IFunctionDbContext functionDbContext)
{
    [Function("TriggerPullServicesWebhook")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "POST")] HttpRequestData req)
    {
        logger.LogInformation("[ApiReceiver] HTTP Trigger Function Started");

        (HttpStatusCode HttpStatusCode, JsonElement.ArrayEnumerator? Result) services = await hsdaApiService.GetServices();
        if (services.HttpStatusCode != HttpStatusCode.OK) return req.CreateResponse(services.HttpStatusCode);

        (HttpStatusCode HttpStatusCode, List<Service> Result) servicesById = await hsdaApiService.GetServicesById(services.Result!.Value);
        if (servicesById.HttpStatusCode != HttpStatusCode.OK) return req.CreateResponse(servicesById.HttpStatusCode);

        try
        {
            await ClearDatabase();
            await UpdateDatabase(servicesById.Result);
        }
        catch (Exception e)
        {
            logger.LogError("{exception}", e.Message);
            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }

        return req.CreateResponse(HttpStatusCode.OK);
    }
    
    [Function("TestLaIngestion")]
    public async Task<HttpResponseData> TestDataIngestionFromFile([HttpTrigger(AuthorizationLevel.Function, "POST")] HttpRequestData req)
    {
        logger.LogInformation("[ApiReceiver] HTTP Trigger Function Started");
        
        // For this prototype only, you will need to create a JSON array of single services and save it to '/data/single_services_as_list.json'
        // You can get the data via internation spec API then get the id's and call '/services/{id}', copy and paste to file.
        // Somerset for example is 'https://api-openreferral.azure-api.net/somersetcouncil/services'
        var service = new DataFileService(); // Only used in this prototype. Production code will call LA API's
        var services = service.GetSingleServicesFromListFile("single_services_as_list.json"); // Only used in this prototype. Production code will call LA API's

        try
        {
            await ClearDatabase();
            await UpdateDatabase(services);
        }
        catch (Exception e)
        {
            logger.LogError("{exception}", e.Message);
            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }

        return req.CreateResponse(HttpStatusCode.OK);
    }

    /*
     * As clearing the Db is a temporary measure until we implement checking for existing IDs, I think this is OK even
     * if it's the slower way of doing it.
     */
    private async Task ClearDatabase()
    {
        logger.LogInformation("Removing all services from the database");
        List<Service> serviceListFromDb = await functionDbContext.ToListAsync(functionDbContext.Services());

        foreach (Service service in serviceListFromDb)
        {
            logger.LogInformation("Removing service from the database, Internal ID = {iId} | Open Referral ID = {oId}",
                service.Id, service.OrId);
            functionDbContext.DeleteService(service);
        }

        await functionDbContext.SaveChangesAsync();
    }

    private async Task UpdateDatabase(List<Service> serviceListFromApi)
    {
        foreach (Service service in serviceListFromApi)
        {
            logger.LogInformation("Adding service with ID {serviceId} to the database", service.OrId);
            functionDbContext.AddService(service);
        }

        logger.LogInformation("Saving changes to the database");
        await functionDbContext.SaveChangesAsync();
    }
}