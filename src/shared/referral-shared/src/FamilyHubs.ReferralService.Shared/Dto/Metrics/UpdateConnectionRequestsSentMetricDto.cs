using System.Net;

namespace FamilyHubs.ReferralService.Shared.Dto.Metrics;

//todo: when we swap to .net 8, use TimeProvider instead
public record UpdateConnectionRequestsSentMetricDto(
    DateTimeOffset RequestTimestamp,
    HttpStatusCode HttpStatusCode,
    long? ConnectionRequestId);
