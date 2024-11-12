using System.Text.Json;
using FamilyHubs.ServiceDirectory.Core.Distance;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using GeoCoordinatePortable;

namespace FamilyHubs.ServiceDirectory.Core.FamilyHubs;

public interface IFamilyHubsProvider
{
    Task LoadFamilyHubsAsync();

    /// <summary>
    /// Gets a list of Family Hubs as ServiceDtos in a given admin area.
    /// </summary>
    /// <param name="adminArea"></param>
    /// <returns></returns>
    IEnumerable<ServiceDto> GetFamilyHubsInAdminArea(string adminArea, double? currLat, double? currLng);
}

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
            Console.WriteLine(familyHub.GetProperty("name").GetString()!);
            
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

            double lat = result.GetProperty("latitude").GetDouble();
            double lng = result.GetProperty("longitude").GetDouble();
            double distance = GetDistance(lat, lng, currLat, currLng);

            ServiceDto familyHubService = new()
            {
                Name = familyHub.GetProperty("name").GetString()!,
                Description = "This is a test description!",
                ServiceType = ServiceType.FamilyExperience,
                Status = ServiceStatusType.Active,
                DeliverableType = DeliverableType.NotSet,
                Distance = distance,
                Locations =
                [
                    new()
                    {
                        LocationTypeCategory = LocationTypeCategory.NotSet,
                        Name = "Address",
                        Latitude = lat,
                        Longitude = lng,
                        Distance = distance,
                        Address1 = "???",
                        City = "???",
                        PostCode = familyHub.GetProperty("postcode").GetString()!,
                        StateProvince = result.GetProperty("admin_district").GetString()!,
                        Country = result.GetProperty("country").GetString()!,
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

    private static double GetDistance(double? latitude1, double? longitude1, double? latitude2, double? longitude2)
    {
        latitude1 ??= 0.0;
        longitude1 ??= 0.0;
        latitude2 ??= 0.0;
        longitude2 ??= 0.0;

        var pin1 = new GeoCoordinate(latitude1.Value, longitude1.Value);
        var pin2 = new GeoCoordinate(latitude2.Value, longitude2.Value);

        return pin1.GetDistanceTo(pin2);
    }
}