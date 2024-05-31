namespace FamilyHubs.ReferralService.Shared.Dto;

public record UserAccountOrganisationDto : DtoBase<long>
{
    public long UserAccountId { get { return (UserAccount != null) ? UserAccount.Id : 0; } }
    public required UserAccountDto UserAccount { get; set; }
    public long OrganisationId { get { return (Organisation != null) ? Organisation.Id : 0; } }
    public required OrganisationDto Organisation { get; set; }
    
}
