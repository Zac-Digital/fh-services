using FamilyHubs.ReferralService.Shared.Dto;

namespace FamilyHubs.ReferralService.Shared.Tests;

public class ReferralOrganisationComparerTests : DtoComparerTestBase<OrganisationDto, string, long>
{
    public ReferralOrganisationComparerTests() : base(new OrganisationDto
    {
        Id = 1,
        Name = "Organisation",
        Description = "Organisation Description",

    }, new OrganisationDto
    {
        Id = 2,
        Name = "Organisation",
        Description = "Organisation Description",

    }, dto => dto.Name)
    {

    }
}
