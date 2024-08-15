namespace FamilyHubs.ServiceDirectory.Data.Entities.Staging;

public class ServicesTemp
{
    public Guid Id { get; set; }
    public required string Json { get; set; }
    public required DateTime LastModified { get; set; }
}
