using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel.Identity;

namespace FamilyHubs.Idam.Core.Queries.GetAccounts;

internal class UserOrganisationsFilterStandard : IUserOrganisationsFilter
{
    private readonly List<OrganisationDto>? _organisationIds;
    public bool Any => _organisationIds != null && _organisationIds.Any();

    public UserOrganisationsFilterStandard()
    {
    }

    public UserOrganisationsFilterStandard(List<OrganisationDto>? organisationIds)
    {
        _organisationIds = organisationIds;
    }

    public Task<IQueryable<Account>> Filter(IQueryable<Account> accountsQuery, string? organisationName)
    {
        var orgs = (_organisationIds ?? new List<OrganisationDto>());
        IEnumerable<OrganisationDto> filtered = orgs;
        if (!string.IsNullOrEmpty(organisationName))
        {
            filtered = orgs.Where(x => x.Name.ToLower().Contains(organisationName.ToLower()));
        }

        var organisationIds = filtered.Select(x => x.Id.ToString());
        return Task.FromResult(accountsQuery.Where(acc =>
            acc.Claims.Any(claim => claim.Name == FamilyHubsClaimTypes.OrganisationId && organisationIds.Contains(claim.Value))
        ));
    }

    public Task<IUserOrganisationsFilter> Requested(long requestedOrganisationId)
    {
        return Task.FromResult((IUserOrganisationsFilter) new UserOrganisationsFilterStandard(
            _organisationIds!.Where(x => x.Id == requestedOrganisationId || x.AssociatedOrganisationId == requestedOrganisationId).ToList()
        ));
    }

    public Task<Dictionary<string, OrganisationDto>> MapFor(IEnumerable<string> orgIds)
    {
        return Task.FromResult(_organisationIds!.ToDictionary(org => org.Id.ToString()));
    }
}