using FamilyHubs.Idams.Maintenance.Core.Services;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idams.Maintenance.Core.ApiClient;

public interface IServiceDirectoryClient
{
    Task<OrganisationDetailsDto?> GetOrganisationById(long id);
    Task<List<OrganisationDto>> GetOrganisations(CancellationToken cancellationToken = default);
}

public class ServiceDirectoryClient : ApiService<ServiceDirectoryClient>, IServiceDirectoryClient
{
    private readonly ICacheService _cacheService;

    public ServiceDirectoryClient(HttpClient client, ICacheService cacheService, ILogger<ServiceDirectoryClient> logger)
        : base(client, logger)
    {
        _cacheService = cacheService;
    }

    public async Task<OrganisationDetailsDto?> GetOrganisationById(long id)
    {
        var request = new HttpRequestMessage();
        request.Method = HttpMethod.Get;
        request.RequestUri = new Uri(Client.BaseAddress + $"api/organisations/{id}");

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        Logger.LogInformation($"{nameof(ServiceDirectoryClient)} Returning Organisation");
        return await DeserializeResponse<OrganisationDetailsDto>(response);
    }

    public async Task<List<OrganisationDto>> GetOrganisations(CancellationToken cancellationToken = default)
    {
        var semaphore = new SemaphoreSlim(1, 1);
        var organisations = await _cacheService.GetOrganisations();
        if (organisations is not null)
            return organisations;

        await semaphore.WaitAsync(cancellationToken);

        var request = new HttpRequestMessage();
        request.Method = HttpMethod.Get;
        request.RequestUri = new Uri(Client.BaseAddress + "api/organisations");

        using var response = await Client.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        organisations = await DeserializeResponse<List<OrganisationDto>>(response, cancellationToken) ?? new List<OrganisationDto>();

        Logger.LogInformation($"{nameof(ServiceDirectoryClient)} Returning  {organisations.Count} Organisations");

        await _cacheService.StoreOrganisations(organisations);

        return organisations;
    }
}
