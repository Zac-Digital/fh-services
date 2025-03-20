namespace FamilyHubs.Referral.Core.Helper;

public static class DistanceConverter
{
    private const double MetersInMiles = 1609.34d;

    public static int MilesToMeters(int miles) => (int)(miles * MetersInMiles);
}