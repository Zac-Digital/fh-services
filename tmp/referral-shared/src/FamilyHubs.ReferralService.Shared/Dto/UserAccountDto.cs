namespace FamilyHubs.ReferralService.Shared.Dto;


public record UserAccountDto : DtoBase<long>
{
    public required string EmailAddress { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Team { get; set; }
    public IList<UserAccountRoleDto>? UserAccountRoles { get; set; }
    public IList<UserAccountServiceDto>? ServiceUserAccounts { get; set; }
    public IList<UserAccountOrganisationDto>? OrganisationUserAccounts { get; set; }

    public override int GetHashCode()
    {
        var result = EqualityComparer<string>.Default.GetHashCode(EmailAddress) * -1521134295;
        if (!string.IsNullOrEmpty(Name))
            result += EqualityComparer<string>.Default.GetHashCode(Name);
        if (!string.IsNullOrEmpty(PhoneNumber))
            result += EqualityComparer<string>.Default.GetHashCode(PhoneNumber);
        if (!string.IsNullOrEmpty(Team))
            result += EqualityComparer<string>.Default.GetHashCode(Team);

        return result;


    }

    public virtual bool Equals(UserAccountDto? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other))
            return true;

        return
            EqualityComparer<string>.Default.Equals(EmailAddress, other.EmailAddress) &&
            EqualityComparer<string>.Default.Equals(Name, other.Name) &&
            EqualityComparer<string>.Default.Equals(PhoneNumber, other.PhoneNumber) &&
            EqualityComparer<string>.Default.Equals(Team, other.Team);
    }
}
