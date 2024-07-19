using System.Text.Json;
using FamilyHubs.Report.Core.Queries.ServiceSearchFacts;
using FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;
using FamilyHubs.Report.Data.Entities;
using FamilyHubs.Report.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.Reports.WeeklyBreakdown;
using NSubstitute;
using Xunit;

namespace FamilyHubs.ReportApi.UnitTests;

public class WhenUsingGetFourWeekBreakdownQuery
{
    private readonly IGetFourWeekBreakdownQuery _getFourWeekBreakdownQuery;

    private readonly DateTime _searchDate = DateTime.Parse("2024-01-31");

    public WhenUsingGetFourWeekBreakdownQuery()
    {
        TimeDim timeDim = new()
        {
            TimeKey = 1
        };

        ServiceSearchesDim serviceSearchesDimOne = new()
        {
            ServiceSearchesKey = 1,
            OrganisationId = 1,
            ServiceTypeId = (byte)ServiceType.InformationSharing
        };

        ServiceSearchesDim serviceSearchesDimTwo = new()
        {
            ServiceSearchesKey = 2,
            OrganisationId = 2,
            ServiceTypeId = (byte)ServiceType.InformationSharing
        };

        DateDim dateDimWeekOne = new()
        {
            DateKey = 1,
            Date = DateTime.Parse("2024-01-01")
        };

        DateDim dateDimWeekTwo = new()
        {
            DateKey = 2,
            Date = DateTime.Parse("2024-01-08")
        };

        DateDim dateDimWeekThree = new()
        {
            DateKey = 3,
            Date = DateTime.Parse("2024-01-15")
        };

        DateDim dateDimWeekFour = new()
        {
            DateKey = 4,
            Date = DateTime.Parse("2024-01-22")
        };

        List<ServiceSearchFact> serviceSearchFactList = new();

        for (int i = 0; i < 4; i++) // Week 1
        {
            serviceSearchFactList.Add(new ServiceSearchFact()
            {
                DateKey = 1,
                DateDim = dateDimWeekOne,
                TimeKey = 1,
                TimeDim = timeDim,
                ServiceSearchesKey = 1,
                ServiceSearchesDim = serviceSearchesDimOne,
            });
        }

        for (int i = 0; i < 8; i++) // Week 2
        {
            serviceSearchFactList.Add(new ServiceSearchFact()
            {
                DateKey = 2,
                DateDim = dateDimWeekTwo,
                TimeKey = 1,
                TimeDim = timeDim,
                ServiceSearchesKey = 1,
                ServiceSearchesDim = serviceSearchesDimOne,
            });
        }

        for (int i = 0; i < 16; i++) // Week 3
        {
            serviceSearchFactList.Add(new ServiceSearchFact()
            {
                DateKey = 3,
                DateDim = dateDimWeekThree,
                TimeKey = 1,
                TimeDim = timeDim,
                ServiceSearchesKey = 2,
                ServiceSearchesDim = serviceSearchesDimTwo,
            });
        }

        for (int i = 0; i < 32; i++) // Week 4
        {
            serviceSearchFactList.Add(new ServiceSearchFact()
            {
                DateKey = 4,
                DateDim = dateDimWeekFour,
                TimeKey = 1,
                TimeDim = timeDim,
                ServiceSearchesKey = 2,
                ServiceSearchesDim = serviceSearchesDimTwo,
            });
        }

        IReportDbContext reportDbContextMock = Substitute.For<IReportDbContext>();

        reportDbContextMock.ServiceSearchFacts.Returns(serviceSearchFactList.AsQueryable());

        reportDbContextMock.CountAsync(Arg.Any<IQueryable<ServiceSearchFact>>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var queryable = callInfo.ArgAt<IQueryable<ServiceSearchFact>>(0);
                return Task.FromResult(queryable.Count());
            });

        IGetServiceSearchFactQuery getServiceSearchFactQueryMock =
            Substitute.For<GetServiceSearchFactQuery>(reportDbContextMock);

        _getFourWeekBreakdownQuery = new GetFourWeekBreakdownQuery(getServiceSearchFactQueryMock);
    }

    [Fact]
    public async Task Then_GetFourWeekBreakdownForAdmin_Should_Return_ExpectedResult()
    {
        WeeklyReportBreakdown expected = new()
        {
            WeeklyReports = new WeeklyReport[]
            {
                new()
                {
                    Date = "1 January to 7 January",
                    SearchCount = 4
                },
                new()
                {
                    Date = "8 January to 14 January",
                    SearchCount = 8
                },
                new()
                {
                    Date = "15 January to 21 January",
                    SearchCount = 16
                },
                new()
                {
                    Date = "22 January to 28 January",
                    SearchCount = 32
                },
            },
            TotalSearchCount = 60
        };

        WeeklyReportBreakdown result =
            await _getFourWeekBreakdownQuery.GetFourWeekBreakdownForAdmin(new SearchBreakdownRequest(_searchDate, ServiceType.InformationSharing));

        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(result));
    }

    [Fact]
    public async Task Then_GetFourWeekBreakdownForLa_OrganisationOne_Should_Return_ExpectedResult()
    {
        WeeklyReportBreakdown expected = new()
        {
            WeeklyReports = new WeeklyReport[]
            {
                new()
                {
                    Date = "1 January to 7 January",
                    SearchCount = 4
                },
                new()
                {
                    Date = "8 January to 14 January",
                    SearchCount = 8
                },
                new()
                {
                    Date = "15 January to 21 January",
                    SearchCount = 0
                },
                new()
                {
                    Date = "22 January to 28 January",
                    SearchCount = 0
                },
            },
            TotalSearchCount = 12
        };

        WeeklyReportBreakdown result =
            await _getFourWeekBreakdownQuery.GetFourWeekBreakdownForLa(new LaSearchBreakdownRequest(_searchDate, ServiceType.InformationSharing, 1));

        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(result));
    }

    [Fact]
    public async Task Then_GetFourWeekBreakdownForLa_OrganisationTwo_Should_Return_ExpectedResult()
    {
        WeeklyReportBreakdown expected = new()
        {
            WeeklyReports = new WeeklyReport[]
            {
                new()
                {
                    Date = "1 January to 7 January",
                    SearchCount = 0
                },
                new()
                {
                    Date = "8 January to 14 January",
                    SearchCount = 0
                },
                new()
                {
                    Date = "15 January to 21 January",
                    SearchCount = 16
                },
                new()
                {
                    Date = "22 January to 28 January",
                    SearchCount = 32
                },
            },
            TotalSearchCount = 48
        };

        WeeklyReportBreakdown result =
            await _getFourWeekBreakdownQuery.GetFourWeekBreakdownForLa(new LaSearchBreakdownRequest(_searchDate, ServiceType.InformationSharing, 2));

        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(result));
    }
}
