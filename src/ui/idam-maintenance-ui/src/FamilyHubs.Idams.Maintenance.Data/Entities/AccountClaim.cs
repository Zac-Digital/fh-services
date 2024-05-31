namespace FamilyHubs.Idams.Maintenance.Data.Entities;

public class AccountClaim : EntityBase<long>
{
    public long AccountId { get; set; }
    public required string Name { get; set; }
    public required string Value { get; set; }
}
