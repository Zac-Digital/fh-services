namespace FamilyHubs.ReferralService.Shared.Dto;

public record UserAccountRoleDto : DtoBase<long>
{
    public long UserAccountId { get { return (UserAccount != null) ? UserAccount.Id : 0; } }
    public virtual required UserAccountDto UserAccount { get; set; }

    public byte RoleId { get { return (Role != null) ? Role.Id : (byte)0; } }
    public required RoleDto Role { get; set; }

    public override int GetHashCode()
    {
        var result = EqualityComparer<long>.Default.GetHashCode(UserAccountId) * -1521134295 +
                     EqualityComparer<long>.Default.GetHashCode(RoleId);
       

        return result;


    }

    public virtual bool Equals(UserAccountRoleDto? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other))
            return true;

        return
            EqualityComparer<long>.Default.Equals(UserAccountId, other.UserAccountId) &&
            EqualityComparer<long>.Default.Equals(RoleId, other.RoleId);
           
    }
}
