using System.Text.Json;
using FamilyHubs.ServiceDirectory.Shared.Dto;

namespace FamilyHubs.Idam.Core.Services;

public interface IServiceDirectoryService
{
    Task<List<OrganisationDto>?> GetAllOrganisations();
    Task<List<OrganisationDto>?> GetOrganisationsByAssociatedId(long id);
    Task<List<OrganisationDto>?> GetOrganisationsByName(string name);
    Task<List<OrganisationDto>?> GetOrganisationsByIds(IEnumerable<long> ids);
}

public class ServiceDirectoryService : IServiceDirectoryService
{
    private readonly HttpClient _httpClient;

    public ServiceDirectoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task<List<OrganisationDto>?> GetOrganisationsByUri(string uri)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_httpClient.BaseAddress + uri)
        };

        using var response = await _httpClient.SendAsync(request);

        var json = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(json))
        {
            throw new Exception("Invalid response from ServiceDirectory Api");
        }

        return JsonSerializer.Deserialize<List<OrganisationDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<List<OrganisationDto>?> GetAllOrganisations()
    {
        return await GetOrganisationsByUri("api/organisations");
    }

    public async Task<List<OrganisationDto>?> GetOrganisationsByAssociatedId(long id)
    {
        return await GetOrganisationsByUri($"api/organisationsByAssociatedOrganisation?id={id}");
    }

    public async Task<List<OrganisationDto>?> GetOrganisationsByName(string name)
    {
        return await GetOrganisationsByUri($"api/organisations?name={name}");
    }

    public async Task<List<OrganisationDto>?> GetOrganisationsByIds(IEnumerable<long> ids)
    {
        var enumerable = ids.ToList();
        if (!enumerable.Any()) return new List<OrganisationDto>();

        var query = string.Join("&ids=", enumerable);
        return await GetOrganisationsByUri($"api/organisations?ids={query}");
    }
}