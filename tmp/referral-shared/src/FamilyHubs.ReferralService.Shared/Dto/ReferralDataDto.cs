namespace FamilyHubs.ReferralService.Shared.Dto;

public class ReferralDataDto
{
    public required ReferralDto ReferralDto { get; set; }
    public required List<UserAccountDto> UserAccounts { get; set; }
}
