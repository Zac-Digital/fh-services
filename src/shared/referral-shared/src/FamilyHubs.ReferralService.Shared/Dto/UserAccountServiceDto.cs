namespace FamilyHubs.ReferralService.Shared.Dto;

public class UserAccountServiceDto
{
    public long UserAccountId { get { return (UserAccount != null) ? UserAccount.Id : 0; } }
    public required UserAccountDto UserAccount { get; set; }

    public long ReferralServiceId { get { return (ReferralService != null) ? ReferralService.Id : 0; } }
    public required ReferralServiceDto ReferralService { get; set; }
}
