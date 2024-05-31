using FamilyHubs.Report.Core.Queries.ServiceSearchFacts;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FamilyHubs.Report.Api.Endpoints;

// TODO: Convert to Mediator Pattern

public class MinimalAdminReportEndPoints
{
    public void RegisterAdminReportEndPoints(WebApplication app)
    {
        app.MapGet("report/service-searches-past-7-days",
            [Authorize(Roles = RoleTypes.DfeAdmin)]
            async (
                DateTime date,
                ServiceType serviceTypeId,
                IGetServiceSearchFactQuery getServiceSearchFactQuery
            ) => await getServiceSearchFactQuery.GetSearchCountForAdmin(date, serviceTypeId, 7));

        app.MapGet("report/service-searches-4-week-breakdown",
            [Authorize(Roles = RoleTypes.DfeAdmin)]
            async (
                DateTime date,
                ServiceType serviceTypeId,
                IGetFourWeekBreakdownQuery getFourWeekBreakdownQuery
            ) => await getFourWeekBreakdownQuery.GetFourWeekBreakdownForAdmin(date, serviceTypeId));

        app.MapGet("report/service-searches-total",
            [Authorize(Roles = RoleTypes.DfeAdmin)]
            async (
                ServiceType serviceTypeId,
                IGetServiceSearchFactQuery getServiceSearchFactQuery
            ) => await getServiceSearchFactQuery.GetTotalSearchCountForAdmin(serviceTypeId));
    }
}
