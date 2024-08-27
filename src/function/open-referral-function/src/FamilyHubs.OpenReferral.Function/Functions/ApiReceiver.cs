using FamilyHubs.OpenReferral.Function.Functions.Repository;
using FamilyHubs.ServiceDirectory.Data.Entities.Staging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.OpenReferral.Function.Functions;

public class ApiReceiver(ILogger<ApiReceiver> logger, IFunctionDbContext dbServiceDirectory)
{
    private readonly string? _connectionApi = Environment.GetEnvironmentVariable("ConnectionApi");

    [Function("ApiReceiver")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "POST")] HttpRequest request)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        HttpClient httpClient = new();
        using HttpResponseMessage response = await httpClient.GetAsync(_connectionApi + "/services");
        string jsonResponse = await response.Content.ReadAsStringAsync();

        logger.LogInformation("{JsonResponse}", jsonResponse);

        ServicesTemp servicesTemp = new()
        {
            Json = jsonResponse,
            LastModified = DateTime.UtcNow
        };

        await dbServiceDirectory.TruncateServicesTempAsync();

        dbServiceDirectory.AddServiceTemp(servicesTemp);
        await dbServiceDirectory.SaveChangesAsync();

        int dbTest = await dbServiceDirectory.ServicesTemp.CountAsync();

        logger.LogInformation("Services Count (Test) = {DbTest}", dbTest);

        return new OkObjectResult("Welcome to Azure Functions!");
    }

}