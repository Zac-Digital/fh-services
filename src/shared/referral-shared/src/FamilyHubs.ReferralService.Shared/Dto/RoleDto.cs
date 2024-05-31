namespace FamilyHubs.ReferralService.Shared.Dto;

public record RoleDto : DtoBase<byte>
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public override int GetHashCode()
    {
        return
            EqualityComparer<string>.Default.GetHashCode(Name) * -1521134295 +
            EqualityComparer<string?>.Default.GetHashCode(Description!) * -1521134295;
    }

    public virtual bool Equals(RoleDto? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other))
            return true;

        return
            EqualityComparer<string>.Default.Equals(Name, other.Name) &&
            EqualityComparer<string?>.Default.Equals(Description, other.Description);
    }
}
