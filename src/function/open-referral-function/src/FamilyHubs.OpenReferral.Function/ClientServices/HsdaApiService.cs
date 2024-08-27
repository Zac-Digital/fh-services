using System.Net;
using FamilyHubs.OpenReferral.Function.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FamilyHubs.OpenReferral.Function.ClientServices;

public class HsdaApiService(ILogger<HsdaApiService> logger)
{
    private readonly HttpClient _httpClient = new()
        { BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiConnection")!) };

    public async Task<(HttpStatusCode, List<ServiceJson>?)> GetServices()
    {
        (HttpStatusCode httpStatusCode, string? jsonResponse) = await HttpGet("/services");

        if (httpStatusCode != HttpStatusCode.OK || jsonResponse is null)
        {
            logger.LogError(
                "Failed to get the service list from /services | Status Code = {httpStatusCode}, JSON = {jsonResponse}",
                httpStatusCode, jsonResponse);
            return (httpStatusCode, null);
        }

        JArray serviceList = JArray.Parse(JObject.Parse(jsonResponse)["contents"]!.ToString());

        if (serviceList.Count == 0)
        {
            logger.LogInformation("Query was OK, but no services were found");
            return (HttpStatusCode.OK, null);
        }

        logger.LogInformation("Found {serviceCount} service(s)..", serviceList.Count);

        List<ServiceJson> serviceJsonList = [];

        foreach (JToken service in serviceList)
        {
            string serviceId = service["id"]!.ToString();
            (httpStatusCode, jsonResponse) = await GetServiceById(serviceId);

            if (httpStatusCode != HttpStatusCode.OK || jsonResponse is null)
            {
                logger.LogWarning(
                    "Failed to get /service/{serviceId} | Status Code = {httpStatusCode}, JSON = {jsonResponse}",
                    serviceId, httpStatusCode, jsonResponse);
                continue;
            }

            serviceJsonList.Add(new ServiceJson { Id = serviceId, Json = jsonResponse });
        }

        return (httpStatusCode, serviceJsonList);
    }

    private async Task<(HttpStatusCode, string?)> GetServiceById(string serviceId)
    {
        logger.LogInformation("Fetching service with id {serviceId}", serviceId);
        return await HttpGet("/services/" + serviceId);
    }

    private async Task<(HttpStatusCode, string?)> HttpGet(string endpoint)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        logger.LogInformation("GET {endpoint} | Status Code = {httpStatusCode}", endpoint, response.StatusCode);
        return (response.StatusCode,
            response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadAsStringAsync() : null);
    }
}