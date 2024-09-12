﻿using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FamilyHubs.Referral.Core.Models;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Dto.Metrics;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Models;

namespace FamilyHubs.Referral.Core.ApiClients;

public interface IOrganisationClientService
{
    Task<List<KeyValuePair<TaxonomyDto, List<TaxonomyDto>>>> GetCategories();

    Task<(
        PaginatedList<ServiceDto> services,
        HttpResponseMessage? response
    )> GetLocalOffers(LocalOfferFilter filter);

    Task<ServiceDto> GetLocalOfferById(string id);

    Task<OrganisationDto?> GetOrganisationDtoByIdAsync(long id);
    
    Task RecordServiceSearch(
        ServiceDirectorySearchEventType eventType,
        string postcode,
        long userId,
        IEnumerable<ServiceDto> services,
        DateTime requestTimestamp,
        DateTime? responseTimestamp,
        HttpStatusCode? responseStatusCode,
        Guid correlationId
    );
}

public class OrganisationClientService : ApiService, IOrganisationClientService
{
    public OrganisationClientService(HttpClient client) : base(client)
    {
    }

    public async Task<List<KeyValuePair<TaxonomyDto, List<TaxonomyDto>>>> GetCategories()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/taxonomies?taxonomyType=ServiceCategory&pageNumber=1&pageSize=99999999"),
        };

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<TaxonomyDto>>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var keyValuePairs = new List<KeyValuePair<TaxonomyDto, List<TaxonomyDto>>>();

        if (retVal == null)
            return keyValuePairs;

        var topLevelCategories = retVal.Items
            .Where(x => x.ParentId == null && x.TaxonomyType == TaxonomyType.ServiceCategory)
            .OrderBy(x => x.Name)
            .ToList();

        foreach (var topLevelCategory in topLevelCategories)
        {
            var subCategories = retVal.Items.Where(x => x.ParentId == topLevelCategory.Id).OrderBy(x => x.Name).ToList();
            var pair = new KeyValuePair<TaxonomyDto, List<TaxonomyDto>>(topLevelCategory, subCategories);
            keyValuePairs.Add(pair);
        }

        return keyValuePairs;
    }

    public async Task<(
        PaginatedList<ServiceDto> services,
        HttpResponseMessage? response
    )> GetLocalOffers(LocalOfferFilter filter)
    {
        if (string.IsNullOrEmpty(filter.Status))
            filter.Status = "Active";

        var urlBuilder = new StringBuilder(
            GetPositionUrl(filter.ServiceType, filter.Latitude, filter.Longitude, filter.Proximity,
                filter.Status, filter.PageNumber, filter.PageSize));

        AddTextToUrl(urlBuilder, filter.Text);

        if (filter.AllChildrenYoungPeople == true)
        {
            urlBuilder.Append("&allChildrenYoungPeople=true");
        }

        AddAgeToUrl(urlBuilder, filter.GivenAge);

        if (filter.ServiceDeliveries is not null)
        {
            urlBuilder.Append($"&serviceDeliveries={filter.ServiceDeliveries}");
        }

        if (filter.IsPaidFor is not null)
        {
            urlBuilder.Append($"&isPaidFor={filter.IsPaidFor.Value}");
        }

        if (filter.TaxonomyIds is not null)
        {
            urlBuilder.Append($"&taxonomyIds={filter.TaxonomyIds}");
        }

        if (filter.DistrictCode is not null)
        {
            urlBuilder.Append($"&districtCode={filter.DistrictCode}");
        }

        if (filter.LanguageCode is not null)
        {
            urlBuilder.Append($"&languages={filter.LanguageCode}");
        }

        if (filter.CanFamilyChooseLocation is not null && filter.CanFamilyChooseLocation == true)
        {
            urlBuilder.Append($"&canFamilyChooseLocation={filter.CanFamilyChooseLocation.Value}");
        }

        if (filter.DaysAvailable is not null)
        {
            urlBuilder.Append($"&days={filter.DaysAvailable}");
        }

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + urlBuilder.ToString()),
        };

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var services = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
               ?? new PaginatedList<ServiceDto>();
        
        return (services, response);
    }

    private static string GetPositionUrl(string? serviceType, double? latitude, double? longitude, double? proximity, string status, int pageNumber, int pageSize)
    {
        return $"api/services-simple?serviceType={serviceType}&status={status}&pageNumber={pageNumber}&pageSize={pageSize}{(
                latitude is not null ? $"&latitude={latitude}" : string.Empty)}{(
                longitude is not null ? $"&longitude={longitude}" : string.Empty)}{(
                proximity is not null ? $"&proximity={proximity}" : string.Empty)}";
    }

    public static void AddAgeToUrl(StringBuilder url, int? givenAge)
    {
        if (givenAge is not null)
        {
            url.AppendLine($"&givenAge={givenAge}");
        }
    }

    public static void AddTextToUrl(StringBuilder url, string? text)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            url.AppendLine($"&text={text}");
        }
    }

    public async Task<ServiceDto> GetLocalOfferById(string id)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + $"api/services-simple/{id}"),
        };

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<ServiceDto>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ArgumentNullException.ThrowIfNull(retVal);

        return retVal;
    }

    public async Task<OrganisationDto?> GetOrganisationDtoByIdAsync(long id)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + $"api/organisations/{id}"),
        };

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        return await JsonSerializer.DeserializeAsync<OrganisationDto>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task RecordServiceSearch(ServiceDirectorySearchEventType eventType, string postcode, long userId,
        IEnumerable<ServiceDto> services, DateTime requestTimestamp, DateTime? responseTimestamp, HttpStatusCode? responseStatusCode,
        Guid correlationId)
    {
        var serviceSearch = new ServiceSearchDto
        {
            SearchPostcode = postcode,
            SearchRadiusMiles = 0, // Unable to search by radius in connect
            ServiceSearchTypeId = ServiceType.InformationSharing,
            RequestTimestamp = requestTimestamp,
            ResponseTimestamp = responseTimestamp,
            HttpResponseCode = (short?)responseStatusCode,
            SearchTriggerEventId = eventType,
            CorrelationId = correlationId.ToString(),
            UserId = userId,
            ServiceSearchResults = services.Select(s => new ServiceSearchResultDto
            {
                ServiceId = s.Id,
            })
        };

        await Client.PostAsJsonAsync("/api/metrics/service-search", serviceSearch);
    }
}
