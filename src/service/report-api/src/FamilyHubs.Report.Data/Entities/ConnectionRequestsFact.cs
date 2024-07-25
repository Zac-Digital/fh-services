using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyHubs.Report.Data.Entities;

public class ConnectionRequestsFact
{
    public long Id { get; init; }

    public int DateKey { get; init; }
    [NotMapped] public DateDim DateDim { get; init; } = null!;

    public int TimeKey { get; init; }
    [NotMapped] public TimeDim TimeDim { get; init; } = null!;

    public long? OrganisationKey { get; init; }
    [NotMapped] public OrganisationDim OrganisationDim { get; init; } = null!;

#if UserAccount
    public long? UserAccountKey { get; init; }
    [NotMapped] public UserAccountDim UserAccountDim { get; init; } = null!;
#endif

    public long ConnectionRequestServiceKey { get; init; }

    public short ConnectionRequestStatusTypeKey { get; init; }

    public long ConnectionRequestId { get; init; }

    public DateTime Created { get; init; }

    public long CreatedBy { get; init; }

    public DateTime Modified { get; init; }

    public long ModifiedBy { get; init; }
}
