using FamilyHubs.ServiceDirectory.Shared.Dto;

namespace FamilyHubs.ServiceDirectory.Core.FamilyHubs;

public interface IFamilyHubsProvider
{
    Task LoadFamilyHubsAsync();

    /// <summary>
    /// Gets a list of Family Hubs as ServiceDtos in a given admin area.
    /// </summary>
    /// <param name="adminArea">The admin area to load Family Hubs for.</param>
    /// <param name="currLat">Current latitude of the requester.</param>
    /// <param name="currLng">Current longitude of the requester.</param>
    /// <returns></returns>
    IEnumerable<ServiceDto> GetFamilyHubsInAdminArea(string adminArea, double? currLat, double? currLng);
}