using FamilyHubs.ReferralService.Shared.Dto;

namespace FamilyHubs.ReferralService.Shared.Tests;

public class ReferralServiceComparerTests : DtoComparerTestBase<ReferralServiceDto, string, long>
{
    public ReferralServiceComparerTests() : base(new ReferralServiceDto
    {
        Id = 1,
        Name = "Organisation",
        Description = "Service Description",
        Url = "www.Service.com",
        OrganisationDto = new OrganisationDto
        {
            Id = 1,
            Name = "Organisation",
            Description = "Organisation Description",
        }

    }, new ReferralServiceDto
    {
        Id = 2,
        Name = "Organisation",
        Description = "Service Description",
        Url = "www.Service.com",
        OrganisationDto = new OrganisationDto
        {
            Id = 2,
            Name = "Organisation",
            Description = "Organisation Description",
        }

    }, dto => dto.Name)
    {

    }
}
