using GeoCoordinatePortable;

namespace FamilyHubs.SharedKernel.Helpers;

public static class LocationHelpers
{
    /// <summary>
    /// Gets the distance in meters between two points. See https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
    /// </summary>
    /// <param name="latitude1"></param>
    /// <param name="longitude1"></param>
    /// <param name="latitude2"></param>
    /// <param name="longitude2"></param>
    /// todo: throw this away and do it in the db using EF, see https://learn.microsoft.com/en-us/ef/core/modeling/spatial
    public static double GetDistance(double? latitude1, double? longitude1, double? latitude2, double? longitude2)
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