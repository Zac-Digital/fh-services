using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using FamilyHubs.Referral.Data.Repository;
using FamilyHubs.ReferralService.Shared.Dto;
using FamilyHubs.ReferralService.Shared.Models;
using FluentAssertions;

namespace FamilyHubs.Referral.FunctionalTests.TestContainers;

public class WhenUsingReferralsApiUnitTests : BaseWhenUsingReferralApiUnitTests
{
    [Fact]
    public async Task ThenReferralsByReferrerAreRetrieved()
    {
        var referrer = ReferralSeedData.SeedReferral().ElementAt(0).UserAccount.EmailAddress;

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(HttpClient.BaseAddress + $"api/referrals/{referrer}?pageNumber=1&pageSize=10"),
        };

        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                new JwtSecurityTokenHandler().WriteToken(Token));

        using var response = await HttpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ReferralDto>>(
            await response.Content.ReadAsStreamAsync(),
            options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Should().NotBeNull();
        retVal.Items.Count.Should().BeGreaterThan(0);
    }
}