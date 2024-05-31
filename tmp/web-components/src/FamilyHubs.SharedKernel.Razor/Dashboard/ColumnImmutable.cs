
namespace FamilyHubs.SharedKernel.Razor.Dashboard;

public enum ColumnType
{
    Standard,
    AlignedRight,
    Numeric
}

public record ColumnImmutable(string DisplayName, string? SortName = null, ColumnType ColumnType = ColumnType.Standard);
