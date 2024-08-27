using FamilyHubs.OpenReferral.Function.Entities;
using Newtonsoft.Json.Linq;

namespace FamilyHubs.OpenReferral.Function.ClientServices;

public class HsdaApiService
{
    private readonly HttpClient _httpClient = new()
        { BaseAddress = new Uri(Environment.GetEnvironmentVariable("ConnectionApi")!) };

    public async Task<ServiceJson[]> GetServices()
    {
        JArray serviceList = JArray.Parse(JObject.Parse(await HttpGet("/services"))["contents"]!.ToString());
        ServiceJson[] serviceJsonList = new ServiceJson[serviceList.Count];

        for (int i = 0; i < serviceList.Count; i++)
        {
            string serviceId = serviceList[i]["id"]!.ToString();
            serviceJsonList[i] = new ServiceJson()
            {
                Id = serviceId,
                Json = await GetServiceById(serviceId)
            };
        }

        return serviceJsonList;
    }

    private async Task<string> GetServiceById(string serviceId) => await HttpGet("/services/" + serviceId);

    private async Task<string> HttpGet(string endpoint)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        return await response.Content.ReadAsStringAsync();
    }
}