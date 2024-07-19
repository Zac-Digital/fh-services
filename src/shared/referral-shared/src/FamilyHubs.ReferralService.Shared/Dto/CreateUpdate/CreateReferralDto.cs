
using FamilyHubs.ReferralService.Shared.Dto.Metrics;

namespace FamilyHubs.ReferralService.Shared.Dto.CreateUpdate;

public record CreateReferralDto(ReferralDto Referral, ConnectionRequestsSentMetricDto Metrics);