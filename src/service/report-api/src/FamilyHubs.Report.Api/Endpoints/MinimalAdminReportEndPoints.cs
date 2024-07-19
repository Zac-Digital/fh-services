using FamilyHubs.Report.Core.Queries.ServiceSearchFacts;
using FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.Identity;
using FluentValidation;
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
                DateTime? date,
                ServiceType? serviceTypeId,
                IGetServiceSearchFactQuery getServiceSearchFactQuery,
                IValidator<SearchCountRequest> validator
            ) =>
            {
                var req = new SearchCountRequest(date, serviceTypeId, 7);
                await validator.ValidateAndThrowAsync(req);
                return await getServiceSearchFactQuery.GetSearchCountForAdmin(req);
            });

        app.MapGet("report/service-searches-4-week-breakdown",
            [Authorize(Roles = RoleTypes.DfeAdmin)]
            async (
                DateTime? date,
                ServiceType? serviceTypeId,
                IGetFourWeekBreakdownQuery getFourWeekBreakdownQuery,
                IValidator<SearchBreakdownRequest> validator
            ) =>
            {
                var req = new SearchBreakdownRequest(date, serviceTypeId);
                await validator.ValidateAndThrowAsync(req);
                return await getFourWeekBreakdownQuery.GetFourWeekBreakdownForAdmin(req);
            });

        app.MapGet("report/service-searches-total",
            [Authorize(Roles = RoleTypes.DfeAdmin)]
            async (
                ServiceType? serviceTypeId,
                IGetServiceSearchFactQuery getServiceSearchFactQuery,
                IValidator<TotalSearchCountRequest> validator
            ) =>
            {
                var req = new TotalSearchCountRequest(serviceTypeId);
                await validator.ValidateAndThrowAsync(req);
                return await getServiceSearchFactQuery.GetTotalSearchCountForAdmin(req);
            });
    }
}
