using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyHubs.Report.Data.Entities;

public class ServiceSearchFact
{
    public long Id { get; init; }

    public int DateKey { get; init; }
    [NotMapped] public DateDim DateDim { get; init; } = null!;

    public int TimeKey { get; init; }
    [NotMapped] public TimeDim TimeDim { get; init; } = null!;

    public int ServiceSearchesKey { get; init; }
    [NotMapped] public ServiceSearchesDim ServiceSearchesDim { get; init; } = null!;

    public long ServiceSearchId { get; init; }

    public DateTime Created { get; init; }

    public DateTime Modified { get; init; }
}
