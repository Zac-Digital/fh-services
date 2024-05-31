using FamilyHubs.ReferralService.Shared.Dto;

namespace FamilyHubs.ReferralService.Shared.Tests;

public class ReferralComparerTests : DtoComparerTestBase<ReferralDto, string, long>
{
    public ReferralComparerTests() : base(new ReferralDto
    {
        Id = 1,
        ReasonForSupport = "Reason For Support",
        EngageWithFamily = "Engage With Family",
        ReasonForDecliningSupport = "Reason For Declining Support",
        ReferrerTelephone = "01234567890",
        RecipientDto = new RecipientDto
        {
            Id = 1,
            Name = "Joe Blogs",
            Email = "JoeBlog@email.com",
            Telephone = "078123456",
            TextPhone = "078123456",
            AddressLine1 = "Address Line 1",
            AddressLine2 = "Address Line 2",
            TownOrCity = "Town or City",
            County = "Country",
            PostCode = "B30 2TV"
        },
        ReferralUserAccountDto = new UserAccountDto
        {
            Id = 1,
            Name = "Bob Referrer",
            EmailAddress = "Bob.Referrer@email.com",
            PhoneNumber = "0122 865 278",
            UserAccountRoles = new List<UserAccountRoleDto>(),
            Team = "Team"
        },
        Status = new ReferralStatusDto
        {
            Id = 1,     
            Name = "New",
            SortOrder = 1,
                    
        },
        ReferralServiceDto = new ReferralServiceDto
        {
            Id = 1,
            Name = "Service",
            Description = "Service Description",
            OrganisationDto = new OrganisationDto
            {
                Id = 1,
                Name = "Organisation",
                Description = "Organisation Description",
            }
        }

    }, new ReferralDto
    {
        Id = 2,
        ReasonForSupport = "Reason For Support",
        EngageWithFamily = "Engage With Family",
        ReasonForDecliningSupport = "Reason For Declining Support",
        ReferrerTelephone = "01234567890",
        RecipientDto = new RecipientDto
        {
            Id = 2,
            Name = "Joe Blogs",
            Email = "JoeBlog@email.com",
            Telephone = "078123456",
            TextPhone = "078123456",
            AddressLine1 = "Address Line 1",
            AddressLine2 = "Address Line 2",
            TownOrCity = "Town or City",
            County = "Country",
            PostCode = "B30 2TV"
        },
        ReferralUserAccountDto = new UserAccountDto
        {
            Id = 2,
            Name = "Bob Referrer",
            EmailAddress = "Bob.Referrer@email.com",
            PhoneNumber = "0122 865 278",
            UserAccountRoles = new List<UserAccountRoleDto>(),
            Team = "Team"
        },
        Status = new ReferralStatusDto
        {
            Id = 1,
            Name = "New",
            SortOrder = 1,
        },
        ReferralServiceDto = new ReferralServiceDto
        {
            Id = 2,
            Name = "Service",
            Description = "Service Description",
            OrganisationDto = new OrganisationDto
            {
                Id = 2,
                Name = "Organisation",
                Description = "Organisation Description",
            }
        }


    }, dto => dto.ReasonForSupport)
    {

    }
}
