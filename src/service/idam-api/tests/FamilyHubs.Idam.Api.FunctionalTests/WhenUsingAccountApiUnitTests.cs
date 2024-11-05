using FamilyHubs.Idam.Core.Commands.Add;
using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.SharedKernel.Identity;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;

namespace FamilyHubs.Idam.Api.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingAccountApiUnitTests : BaseWhenUsingApiUnitTests
{
    private const string Controller = "Account";

    [Theory]
    [InlineData(RoleTypes.DfeAdmin, RoleTypes.DfeAdmin, "Cannot create DfeAdmin via API")]
    public async Task InvalidAddRequest_ThenExpectedErrorReturned(string createRole, string bearerRole, string expectedMessage)
    {
        //  Arrange
        var requestContent = new AddAccountCommand
        {
            Name = "Full Name",
            Email = "some@domain.com",
            PhoneNumber = "1234567890",
            Claims = new List<AccountClaim> { new AccountClaim { Name = FamilyHubsClaimTypes.Role, Value = createRole } }
        };

        var request = CreatePostRequest(Controller, requestContent, bearerRole);

        //  Act
        using var response = await Client!.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        //  Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var errorObject = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
        errorObject?.Detail.Should().Contain(expectedMessage);
    }

    [Fact]
    public async Task ValidAddRequest_ThenTheAccountIsCreated()
    {
        //  Arrange
        var requestContent = new AddAccountCommand
        {
            Name = "Full Name",
            Email = "some@domain.com",
            PhoneNumber = "1234567890",
            Claims = new List<AccountClaim> { new AccountClaim { Name = FamilyHubsClaimTypes.Role, Value = RoleTypes.LaManager} }
        };

        var request = CreatePostRequest(Controller, requestContent, RoleTypes.DfeAdmin);

        //  Act
        using var response = await Client!.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        //  Assert
        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        responseContent.Should().Be("Account Created");
    }

    [Fact]
    public async Task ThenTheAccountIsRetrieved()
    {
        //  Arrange
        var expected = TestDataProvider.GetTestAccount();
        var request = CreateGetRequest($"{Controller}?email={TestDataProvider.AccountEmail}", RoleTypes.DfeAdmin);

        //  Act
        using var response = await Client!.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        //  Assert
        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var actual = JsonConvert.DeserializeObject<Account>(responseContent);


        actual.Should().BeEquivalentTo(expected, options => options
            .Excluding(info => info.Claims)
            .Excluding(info => info.Id)
            .Excluding(info => info.Created)
            .Excluding(info => info.CreatedBy)
            .Excluding(info => info.LastModified)
            .Excluding(info => info.LastModifiedBy)
        );
    }

    public class ErrorResponse
    {
        public string? Detail { get; set; }
    }
}
