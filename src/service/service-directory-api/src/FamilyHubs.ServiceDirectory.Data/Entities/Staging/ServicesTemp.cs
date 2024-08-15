namespace FamilyHubs.ServiceDirectory.Data.Entities.Staging;

public class ServicesTemp
{
    public long Id { get; set; }
    public required string Json { get; set; }
    public required DateTime LastModified { get; set; }
}
