using FamilyHubs.Report.Core.Queries.ServiceSearchFacts;
using FamilyHubs.Report.Data.Entities;
using FamilyHubs.Report.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using NSubstitute;
using Xunit;

namespace FamilyHubs.ReportApi.UnitTests;

public class WhenUsingGetServiceSearchFactQuery
{
    private readonly IGetServiceSearchFactQuery _getServiceSearchFactQuery;

    public WhenUsingGetServiceSearchFactQuery()
    {
        List<DateDim> dateDimList = new()
        {
            new DateDim
            {
                DateKey = 0,
                Date = DateTime.Parse("2024-08-08")
            },
            new DateDim
            {
                DateKey = 1,
                Date = DateTime.Parse("2024-08-04")
            },
            new DateDim
            {
                DateKey = 2,
                Date = DateTime.Parse("2024-06-08")
            }
        };

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

        List<ServiceSearchFact> serviceSearchFactList = new()
        {
            new ServiceSearchFact
            {
                DateKey = 0,
                DateDim = dateDimList[0],
                TimeKey = 1,
                TimeDim = timeDim,
                ServiceSearchesKey = 1,
                ServiceSearchesDim = serviceSearchesDimOne
            },
            new ServiceSearchFact
            {
                DateKey = 0,
                DateDim = dateDimList[0],
                TimeKey = 1,
                TimeDim = timeDim,
                ServiceSearchesKey = 1,
                ServiceSearchesDim = serviceSearchesDimOne
            },
            new ServiceSearchFact
            {
                DateKey = 0,
                DateDim = dateDimList[0],
                TimeKey = 1,
                TimeDim = timeDim,
                ServiceSearchesKey = 1,
                ServiceSearchesDim = serviceSearchesDimOne
            },
            new ServiceSearchFact
            {
                DateKey = 1,
                DateDim = dateDimList[1],
                TimeKey = 1,
                TimeDim = timeDim,
                ServiceSearchesKey = 2,
                ServiceSearchesDim = serviceSearchesDimTwo
            },
            new ServiceSearchFact
            {
                DateKey = 2,
                DateDim = dateDimList[2],
                TimeKey = 1,
                TimeDim = timeDim,
                ServiceSearchesKey = 2,
                ServiceSearchesDim = serviceSearchesDimTwo
            }
        };

        IReportDbContext reportDbContextMock = Substitute.For<IReportDbContext>();

        reportDbContextMock.ServiceSearchFacts.Returns(serviceSearchFactList.AsQueryable());

        reportDbContextMock.CountAsync(Arg.Any<IQueryable<ServiceSearchFact>>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var queryable = callInfo.ArgAt<IQueryable<ServiceSearchFact>>(0);
                return Task.FromResult(queryable.Count());
            });

        _getServiceSearchFactQuery = new GetServiceSearchFactQuery(reportDbContextMock);
    }

    [Theory]
    [InlineData("2024-08-08", 1, 3)]
    [InlineData("2024-08-04", 1, 1)]
    [InlineData("2024-06-08", 1, 1)]
    [InlineData("2024-08-08", 7, 4)]
    [InlineData("2024-08-08", 365, 5)]
    [InlineData("2024-08-04", 60, 2)]
    [InlineData("2024-06-08", 365, 1)]
    [InlineData("2024-01-01", 365, 0)]
    [InlineData("2024-12-31", 365, 5)]
    [InlineData("2024-12-31", 0, 0)]
    public async Task Then_GetSearchCountForAdmin_Should_Return_ExpectedResult(string dateStr, int days,
        int expected)
    {
        DateTime dateTime = DateTime.Parse(dateStr);

        int result = await _getServiceSearchFactQuery.GetSearchCountForAdmin(dateTime, ServiceType.InformationSharing, days);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("2024-08-08", 1, 3)]
    [InlineData("2024-08-04", 1, 0)]
    [InlineData("2024-06-08", 1, 0)]
    [InlineData("2024-08-08", 7, 3)]
    [InlineData("2024-08-08", 365, 3)]
    [InlineData("2024-08-04", 60, 0)]
    [InlineData("2024-06-08", 365, 0)]
    [InlineData("2024-01-01", 365, 0)]
    [InlineData("2024-12-31", 365, 3)]
    [InlineData("2024-12-31", 0, 0)]
    public async Task Then_GetSearchCountForLa_Should_Return_ExpectedResult(string dateStr, int days, int expected)
    {
        DateTime dateTime = DateTime.Parse(dateStr);

        int result = await _getServiceSearchFactQuery.GetSearchCountForLa(dateTime, ServiceType.InformationSharing, 1, days);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ServiceType.InformationSharing, 5)]
    [InlineData(ServiceType.FamilyExperience, 0)]
    public async Task Then_GetTotalSearchCountForAdmin_Should_Return_ExpectedResult(ServiceType serviceTypeId, int expected)
    {
        int result = await _getServiceSearchFactQuery.GetTotalSearchCountForAdmin(serviceTypeId);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, ServiceType.InformationSharing, 3)]
    [InlineData(2, ServiceType.InformationSharing, 2)]
    [InlineData(1, ServiceType.FamilyExperience, 0)]
    [InlineData(2, ServiceType.FamilyExperience, 0)]
    public async Task Then_GetTotalSearchCountForLa_Should_Return_ExpectedResult(long laOrgId, ServiceType serviceTypeId, int expected)
    {
        int result = await _getServiceSearchFactQuery.GetTotalSearchCountForLa(laOrgId, serviceTypeId);

        Assert.Equal(expected, result);
    }
}
