namespace FamilyHubs.ServiceDirectory.Shared.Display;

public static class AgeDisplayExtensions
{
    public static string AgeToString(int age)
    {
        return age == 127 ? "25+" : age.ToString();
    }
}
