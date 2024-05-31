namespace FamilyHubs.ReferralService.Shared.Models;

public record ReferralResponse
{
    public required long Id { get; set; }
    public required string ServiceName { get; set; }
    public required long OrganisationId { get; set; }
}