using System.Net;
using FamilyHubs.OpenReferral.Function.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FamilyHubs.OpenReferral.Function.ClientServices;

public class HsdaApiService(ILogger<HsdaApiService> logger)
{
    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiConnection")!) };

    public async Task<(HttpStatusCode, ServiceJson[]?)> GetServices()
    {
        (HttpStatusCode httpStatusCode, string? jsonResponse) = await HttpGet("/services");

        if (httpStatusCode != HttpStatusCode.OK) return (httpStatusCode, null);
        if (jsonResponse is null)
        {
            logger.LogError("JSON response for /services is null");
            return (httpStatusCode, null);
        }

        JArray serviceList = JArray.Parse(JObject.Parse(jsonResponse)["contents"]!.ToString());

        if (serviceList.Count == 0)
        {
            logger.LogInformation("Query was OK, but no services were found");
            return (HttpStatusCode.OK, null);
        }

        logger.LogInformation("Found {serviceCount} service(s)..", serviceList.Count);

        ServiceJson[] serviceJsonList = new ServiceJson[serviceList.Count];

        for (int i = 0; i < serviceList.Count; i++)
        {
            string serviceId = serviceList[i]["id"]!.ToString();

            logger.LogInformation("Fetching service with id {serviceId}", serviceId);
            (httpStatusCode, jsonResponse) = await GetServiceById(serviceId);

            if (httpStatusCode != HttpStatusCode.OK)
            {
                logger.LogWarning("Failed to get service information for service with ID {serviceId}, continuing..", serviceId);
                continue;
            }

            serviceJsonList[i] = new ServiceJson
            {
                Id = serviceId,
                Json = jsonResponse!
            };
        }

        return (httpStatusCode, serviceJsonList);
    }

    private async Task<(HttpStatusCode, string?)> GetServiceById(string serviceId) => await HttpGet("/services/" + serviceId);

    private async Task<(HttpStatusCode, string?)> HttpGet(string endpoint)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        logger.LogInformation("GET {endpoint} returned Status Code -> {httpStatusCode}", endpoint, response.StatusCode);
        return (response.StatusCode, response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadAsStringAsync() : null);
    }
}