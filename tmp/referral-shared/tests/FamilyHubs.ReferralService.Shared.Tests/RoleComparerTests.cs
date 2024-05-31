using FamilyHubs.ReferralService.Shared.Dto;

namespace FamilyHubs.ReferralService.Shared.Tests;


public class RoleComparerTests : DtoComparerTestBase<RoleDto, string, byte>
{
    public RoleComparerTests() : base(new RoleDto
    {
        Id = 1,
        Name = "New",
        Description = "Description",
       
    }, new RoleDto
    {
        Id = 2,
        Name = "New",
        Description = "Description",


    }, dto => dto.Name)
    {

    }   
}
