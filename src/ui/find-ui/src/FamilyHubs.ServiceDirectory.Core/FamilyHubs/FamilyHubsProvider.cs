using System.Text.Json;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.Helpers;
using GeoCoordinatePortable;

namespace FamilyHubs.ServiceDirectory.Core.FamilyHubs;

public class FamilyHubsProvider : IFamilyHubsProvider
{
    private const string FamilyHubsJsonPath = "./family-hubs.json";

    private JsonDocument? _data = null;

    public async Task LoadFamilyHubsAsync()
    {
        string json = await File.ReadAllTextAsync(FamilyHubsJsonPath);
        _data = JsonDocument.Parse(json);
    }

    public IEnumerable<ServiceDto> GetFamilyHubsInAdminArea(string adminArea, double? currLat, double? currLng)
    {
        if (_data is null)
            throw new InvalidOperationException($"{nameof(LoadFamilyHubsAsync)} must be called beforehand.");

        List<ServiceDto> familyHubs = [];
        
        foreach (JsonElement familyHub in _data.RootElement.EnumerateArray())
        {
            int status = familyHub
                .GetProperty("lookup")
                .GetProperty("status")
                .GetInt32();
            if (status != 200)
                continue;

            JsonElement result = familyHub
                .GetProperty("lookup")
                .GetProperty("result");
            
            string familyHubAdminArea = result
                .GetProperty("codes")
                .GetProperty("admin_district")
                .GetString()!;
            
            // If iterated FamilyHub isn't in the admin area, skip
            if (!string.Equals(adminArea, familyHubAdminArea, StringComparison.OrdinalIgnoreCase))
                continue;

            string name = familyHub.GetProperty("name").GetString()!;
            double lat = result.GetProperty("latitude").GetDouble();
            double lng = result.GetProperty("longitude").GetDouble();
            double distance = LocationHelpers. GetDistance(lat, lng, currLat, currLng);
            string postcode = familyHub.GetProperty("postcode").GetString()!;
            string adminDistrict = result.GetProperty("admin_district").GetString()!;
            string country = result.GetProperty("country").GetString()!;
            
            ServiceDto familyHubService = new()
            {
                Name = name,
                Description = "",
                ServiceType = ServiceType.FamilyExperience,
                Status = ServiceStatusType.Active,
                DeliverableType = DeliverableType.NotSet,
                Distance = distance,
                Locations =
                [
                    new()
                    {
                        LocationTypeCategory = LocationTypeCategory.FamilyHub,
                        Name = name,
                        Latitude = lat,
                        Longitude = lng,
                        Distance = distance,
                        Address1 = "",
                        City = "",
                        PostCode = postcode,
                        StateProvince = adminDistrict,
                        Country = country,
                        LocationType = LocationType.Physical,
                        Region = null,
                        AccessibilityForDisabilities = [],
                        Schedules = [],
                        Contacts = []
                    }
                ],
                Schedules = [],
                Contacts = [],
                ServiceAtLocations = [],
                Eligibilities = [],
                Fundings = [],
                CostOptions = [],
                Languages = [],
                ServiceAreas = [],
            };

            familyHubs.Add(familyHubService);
        }

        return familyHubs;
    }
}