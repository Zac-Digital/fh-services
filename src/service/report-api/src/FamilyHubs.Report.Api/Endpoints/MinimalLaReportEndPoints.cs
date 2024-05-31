using FamilyHubs.Report.Core.Queries.ServiceSearchFacts;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FamilyHubs.Report.Api.Endpoints;

// TODO: Convert to Mediator Pattern

public class MinimalLaReportEndPoints
{
    public void RegisterLaReportEndPoints(WebApplication app)
    {
        app.MapGet("report/service-searches-past-7-days/organisation/{laOrgId:long}",
            [Authorize(Roles = RoleGroups.LaManagerOrDualRole)]
            async (
                DateTime date,
                ServiceType serviceTypeId,
                long laOrgId,
                IGetServiceSearchFactQuery getServiceSearchFactQuery
            ) => await getServiceSearchFactQuery.GetSearchCountForLa(date, serviceTypeId, laOrgId, 7));

        app.MapGet("report/service-searches-4-week-breakdown/organisation/{laOrgId:long}",
            [Authorize(Roles = RoleGroups.LaManagerOrDualRole)]
            async (
                DateTime date, 
                ServiceType serviceTypeId, 
                long laOrgId,
                IGetFourWeekBreakdownQuery getFourWeekBreakdownQuery
            ) => await getFourWeekBreakdownQuery.GetFourWeekBreakdownForLa(date, serviceTypeId, laOrgId));

        app.MapGet("report/service-searches-total/organisation/{laOrgId:long}",
            [Authorize(Roles = RoleGroups.LaManagerOrDualRole)]
            async (
                long laOrgId,
                ServiceType serviceTypeId,
                IGetServiceSearchFactQuery getServiceSearchFactQuery
            ) => await getServiceSearchFactQuery.GetTotalSearchCountForLa(laOrgId, serviceTypeId));
    }
}
