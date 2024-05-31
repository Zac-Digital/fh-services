using FamilyHubs.ReferralService.Shared.Dto;

namespace FamilyHubs.Referral.Core.Interfaces.Commands;

public interface IUpdateUserAccountCommand
{
    UserAccountDto UserAccount { get; }
}
