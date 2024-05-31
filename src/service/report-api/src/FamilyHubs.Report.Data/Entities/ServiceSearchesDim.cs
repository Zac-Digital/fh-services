namespace FamilyHubs.Report.Data.Entities;

public class ServiceSearchesDim
{
    public int ServiceSearchesKey { get; init; }

    public long ServiceSearchId { get; init; }

    public byte ServiceTypeId { get; init; }

    public string ServiceTypeName { get; init; } = null!;

    public long EventId { get; init; }

    public string EventName { get; init; } = null!;

    public long? UserId { get; init; }

    public string? UserName { get; init; }

    public string? UserEmail { get; init; }

    public byte? RoleTypeId { get; init; }

    public string? RoleTypeName { get; init; }

    public long? OrganisationId { get; init; }

    public string? OrganisationName { get; init; }

    public byte? OrganisationTypeId { get; init; }

    public string? OrganisationTypeName { get; init; }

    public string PostCode { get; init; } = null!;

    public byte SearchRadiusMiles { get; init; }

    public DateTime HttpRequestTimestamp { get; init; }

    public string? HttpRequestCorrelationId { get; init; }

    public short? HttpResponseCode { get; init; }

    public DateTime? HttpResponseTimestamp { get; init; }

    public DateTime Created { get; init; }

    public DateTime Modified { get; init; }
}
