using System.Text.Json;
using FamilyHubs.ServiceDirectory.Shared.Dto;

namespace FamilyHubs.Idam.Core.Services;

public interface IServiceDirectoryService
{
    Task<List<OrganisationDto>?> GetAllOrganisations();
    Task<List<OrganisationDto>?> GetOrganisationsByAssociatedId(long id);
}

public class ServiceDirectoryService : IServiceDirectoryService
{
    private readonly HttpClient _httpClient;

    public ServiceDirectoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<OrganisationDto>?> GetAllOrganisations()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_httpClient.BaseAddress + $"api/organisations")
        };

        using var response = await _httpClient.SendAsync(request);

        var json = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(json))
        {
            throw new Exception("Invalid response from ServiceDirectory Api");
        }

        return JsonSerializer.Deserialize<List<OrganisationDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    }

    public async Task<List<OrganisationDto>?> GetOrganisationsByAssociatedId(long id)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_httpClient.BaseAddress + $"api/organisationsByAssociatedOrganisation?id={id}")
        };

        using var response = await _httpClient.SendAsync(request);

        var json = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(json))
        {
            throw new Exception("Invalid response from ServiceDirectory Api");
        }

        return JsonSerializer.Deserialize<List<OrganisationDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}