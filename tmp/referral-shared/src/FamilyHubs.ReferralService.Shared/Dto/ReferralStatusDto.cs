namespace FamilyHubs.ReferralService.Shared.Dto;

public record ReferralStatusDto : DtoBase<byte>
{
    public required string Name { get; set; }

    public byte SortOrder { get; set; }
    public byte SecondrySortOrder { get; set; }

    public override int GetHashCode()
    {
        return
            EqualityComparer<string>.Default.GetHashCode(Name) * -1521134295 +
            EqualityComparer<byte>.Default.GetHashCode(SortOrder) * -1521134295 +
            EqualityComparer<byte>.Default.GetHashCode(SecondrySortOrder) * -1521134295;
    }

    public virtual bool Equals(ReferralStatusDto? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other))
            return true;

        return
            EqualityComparer<string>.Default.Equals(Name, other.Name) &&
            EqualityComparer<byte>.Default.Equals(SortOrder, other.SortOrder) &&
            EqualityComparer<byte>.Default.Equals(SecondrySortOrder, other.SecondrySortOrder);
    }
}
