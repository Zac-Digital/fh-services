using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;

namespace FamilyHubs.Idam.Core.Queries.GetAccounts;

internal interface IUserOrganisationsFilter
{
    bool Any { get; }
    Task<IQueryable<Account>> Filter(IQueryable<Account> accountsQuery, string? organisationName);
    Task<IUserOrganisationsFilter> Requested(long requestedOrganisationId);
    Task<Dictionary<string, OrganisationDto>> MapFor(IEnumerable<string> orgIds);
}