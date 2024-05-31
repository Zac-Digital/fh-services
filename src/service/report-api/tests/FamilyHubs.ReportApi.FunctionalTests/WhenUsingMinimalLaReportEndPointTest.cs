using System.Net;
using FamilyHubs.SharedKernel.Identity;
using FamilyHubs.SharedKernel.Reports.WeeklyBreakdown;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FamilyHubs.ReportApi.FunctionalTests;

public class WhenUsingMinimalLaReportEndPointTest : BaseEndPointTest, IAsyncLifetime
{
    public async Task InitializeAsync() => await InitialiseDatabase();

    [Theory]
    [InlineData("2024-01-07", 1, "7")]
    [InlineData("2024-01-14", 1, "7")]
    [InlineData("2024-01-07", 2, "0")]
    [InlineData("2024-01-14", 2, "0")]
    [InlineData("2024-02-11", 1, "0")]
    [InlineData("2024-02-18", 1, "0")]
    [InlineData("2024-02-11", 2, "7")]
    [InlineData("2024-02-18", 2, "7")]
    [InlineData("2024-03-31", 1, "0")]
    [InlineData("2024-03-31", 2, "0")]
    public async Task Then_SevenDailySearches_La_ShouldBeCorrect(string dateStr, long laOrgId, string expected)
    {
        HttpResponseMessage httpResponseMessage =
            await Client.SendAsync(CreateHttpGetRequest(
                $"report/service-searches-past-7-days/organisation/{laOrgId}?date={dateStr}&serviceTypeId={ServiceTypeId}",
                RoleTypes.LaManager));

        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);

        string result = await httpResponseMessage.Content.ReadAsStringAsync();
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task Then_FourWeekBreakdown_La_OrganisationOne_ShouldBeCorrect()
    {
        WeeklyReportBreakdown wRbDto = new()
        {
            WeeklyReports = new[]
            {
                new WeeklyReport { Date = "1 January to 7 January", SearchCount = 7 },
                new WeeklyReport { Date = "8 January to 14 January", SearchCount = 7 },
                new WeeklyReport { Date = "15 January to 21 January", SearchCount = 7 },
                new WeeklyReport { Date = "22 January to 28 January", SearchCount = 7 }
            },
            TotalSearchCount = 28
        };

        string expectedJsonStr = JsonSerializer.Serialize(wRbDto, JsonOptions);

        const int laOrgId = 1;
        const string dateStr = "2024-01-31";

        HttpResponseMessage httpResponseMessage =
            await Client.SendAsync(CreateHttpGetRequest(
                $"report/service-searches-4-week-breakdown/organisation/{laOrgId}?date={dateStr}&serviceTypeId={ServiceTypeId}",
                RoleTypes.LaDualRole));

        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);

        string result = await httpResponseMessage.Content.ReadAsStringAsync();
        Assert.Equal(expectedJsonStr, result);
    }

    [Fact]
    public async Task Then_FourWeekBreakdown_La_OrganisationTwo_ShouldBeCorrect()
    {
        WeeklyReportBreakdown wRbDto = new()
        {
            WeeklyReports = new[]
            {
                new WeeklyReport { Date = "29 January to 4 February", SearchCount = 4 },
                new WeeklyReport { Date = "5 February to 11 February", SearchCount = 7 },
                new WeeklyReport { Date = "12 February to 18 February", SearchCount = 7 },
                new WeeklyReport { Date = "19 February to 25 February", SearchCount = 7 }
            },
            TotalSearchCount = 25
        };

        string expectedJsonStr = JsonSerializer.Serialize(wRbDto, JsonOptions);

        const int laOrgId = 2;
        const string dateStr = "2024-02-29";

        HttpResponseMessage httpResponseMessage =
            await Client.SendAsync(CreateHttpGetRequest(
                $"report/service-searches-4-week-breakdown/organisation/{laOrgId}?date={dateStr}&serviceTypeId={ServiceTypeId}",
                RoleTypes.LaManager));

        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);

        string result = await httpResponseMessage.Content.ReadAsStringAsync();
        Assert.Equal(expectedJsonStr, result);
    }

    [Theory]
    [InlineData(1, "31")]
    [InlineData(2, "29")]
    public async Task Then_TotalSearchCount_La_ShouldBeCorrect(long laOrgId, string expected)
    {
        HttpResponseMessage httpResponseMessage =
            await Client.SendAsync(CreateHttpGetRequest(
                $"report/service-searches-total/organisation/{laOrgId}?serviceTypeId={ServiceTypeId}",
                RoleTypes.LaDualRole));

        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);

        string result = await httpResponseMessage.Content.ReadAsStringAsync();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, RoleTypes.DfeAdmin)]
    [InlineData(2, RoleTypes.VcsManager)]
    [InlineData(1, RoleTypes.LaProfessional)]
    [InlineData(2, RoleTypes.VcsProfessional)]
    [InlineData(1, RoleTypes.VcsDualRole)]
    [InlineData(2, RoleTypes.ServiceAccount)]
    public async Task Then_WrongRole_Will_BeForbidden(long laOrgId, string roleType)
    {
        HttpResponseMessage httpResponseMessage =
            await Client.SendAsync(CreateHttpGetRequest(
                $"report/service-searches-total/organisation/{laOrgId}?serviceTypeId={ServiceTypeId}",
                roleType));

        Assert.Equal(HttpStatusCode.Forbidden, httpResponseMessage.StatusCode);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
