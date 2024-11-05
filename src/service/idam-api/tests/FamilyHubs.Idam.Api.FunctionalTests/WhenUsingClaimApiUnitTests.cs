using FamilyHubs.Idam.Core.Commands.Add;
using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.SharedKernel.Identity;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;

namespace FamilyHubs.Idam.Api.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingClaimApiUnitTests : BaseWhenUsingApiUnitTests
{
    private const string Controller = "AccountClaims";

    [Fact]
    public async Task ThenTheClaimIsRetrieved()
    {
        //  Arrange
        var request = CreateGetRequest($"{Controller}/GetAccountClaimsByEmail?email={TestDataProvider.AccountEmail}");

        //  Act
        using var response = await Client!.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        //  Assert
        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var actual = JsonConvert.DeserializeObject<List<AccountClaim>>(responseContent);

        var expected = TestDataProvider.GetTestAccountClaims().ToList();
        expected[0].AccountId = 1;
        expected.AddRange(new[]
        {
            new AccountClaim { AccountId = 1, Name = "OpenId", Value = "TestOpenId" },
            new AccountClaim {  AccountId = 1, Name = "Name", Value = "Test Name" },
            new AccountClaim { AccountId = 1, Name = "Email", Value = "Test@test.com", },
            new AccountClaim { AccountId = 1, Name = "PhoneNumber", Value = "01234567890", },
            new AccountClaim { AccountId = 1, Name = "AccountStatus", Value = "Active", },
            new AccountClaim { AccountId = 1, Name = "AccountId", Value = "1", }
        });
        actual.Should().BeEquivalentTo(expected, options => options
            .Excluding(info => info.Id)
            .Excluding(info => info.Created)
            .Excluding(info => info.CreatedBy)
            .Excluding(info => info.LastModified)
            .Excluding(info => info.LastModifiedBy)
        );
    }

    [Fact]
    public async Task ThenTheClaimIsCreated()
    {
        //  Arrange
        var requestContent = new AddClaimCommand
        {
            AccountId = 1,
            Name = "ClaimName1",
            Value = "ClaimValue2"
        };
        var request = CreatePostRequest($"{Controller}/AddClaim", requestContent, RoleTypes.DfeAdmin);

        //  Act
        using var response = await Client!.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        //  Assert
        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        int.Parse(responseContent).Should().Be(1);
    }

    [Fact]
    public async Task ThenTheClaimIsUpdated()
    {
        //  Arrange
        var requestContent = new
        {
            AccountId = 1,
            Name = "ClaimNameUpdated",
            Value = "ClaimValueUpdated"
        };
        var request = CreatePutRequest($"{Controller}/UpdateClaim", requestContent,RoleTypes.DfeAdmin);

        //  Act
        using var response = await Client!.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        //  Assert
        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        long.Parse(responseContent).Should().Be(1);
    }

    [Fact]
    public async Task ThenTheClaimIsDeleted()
    {
        //  Arrange
        var requestContent = new
        {
            AccountId = 1,
            Name = "ClaimName"
        };
        var request = CreateDeleteRequest($"{Controller}/DeleteClaim", requestContent, RoleTypes.DfeAdmin);

        //  Act
        using var response = await Client!.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        //  Assert
        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        bool.Parse(responseContent).Should().Be(true);
    }

    [Fact]
    public async Task ThenAllTheClaimsAreDeleted()
    {
        //  Arrange
        var requestContent = new { accountId = 1 };
        var request = CreateDeleteRequest($"{Controller}/DeleteAllClaims", requestContent, RoleTypes.DfeAdmin);

        //  Act
        using var response = await Client!.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        //  Assert
        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        bool.Parse(responseContent).Should().Be(true);
    }
}