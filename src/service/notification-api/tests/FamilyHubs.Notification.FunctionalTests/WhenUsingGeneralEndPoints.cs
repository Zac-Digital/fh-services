using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;

//Only run locally
namespace FamilyHubs.Notification.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingGeneralEndPoints : BaseWhenUsingOpenReferralApiUnitTests
{
    [Fact]
    public async Task ThenGetInfo()
    {
        const string expectedStart = "Version: 1.0.0";

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{Client!.BaseAddress}api/info"),
        };

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(Token));

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        var teststring = stringResult.Substring(0, expectedStart.Length);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        ArgumentNullException.ThrowIfNull(stringResult);
        teststring.Trim().Should().Be(expectedStart.Trim());
    }
}
