using FamilyHubs.Report.Core.Queries.ServiceSearchFacts;
using FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace FamilyHubs.Report.Api.Endpoints;

// TODO: Convert to Mediator Pattern

public class MinimalLaReportEndPoints
{
    private const string AllowedRoles = $"{RoleGroups.LaManagerOrDualRole},{RoleTypes.VcsManager}";
    
    public void RegisterLaReportEndPoints(WebApplication app)
    {
        app.MapGet("report/service-searches-past-7-days/organisation/{laOrgId:long}",
            [Authorize(Roles = AllowedRoles)]
            async (
                DateTime? date,
                ServiceType? serviceTypeId,
                long? laOrgId,
                IGetServiceSearchFactQuery getServiceSearchFactQuery,
                IValidator<LaSearchCountRequest> validator
            ) =>
            {
                var req = new LaSearchCountRequest(date, serviceTypeId, laOrgId, 7);
                await validator.ValidateAndThrowAsync(req);
                return await getServiceSearchFactQuery.GetSearchCountForLa(req);
            });

        app.MapGet("report/service-searches-4-week-breakdown/organisation/{laOrgId:long}",
            [Authorize(Roles = AllowedRoles)]
            async (
                DateTime? date, 
                ServiceType? serviceTypeId, 
                long laOrgId,
                IGetFourWeekBreakdownQuery getFourWeekBreakdownQuery,
                IValidator<LaSearchBreakdownRequest> validator
            ) =>
            {
                var req = new LaSearchBreakdownRequest(date, serviceTypeId, laOrgId);
                await validator.ValidateAndThrowAsync(req);
                return await getFourWeekBreakdownQuery.GetFourWeekBreakdownForLa(req);
            });

        app.MapGet("report/service-searches-total/organisation/{laOrgId:long}",
            [Authorize(Roles = AllowedRoles)]
            async (
                long? laOrgId,
                ServiceType? serviceTypeId,
                IGetServiceSearchFactQuery getServiceSearchFactQuery,
                IValidator<LaTotalSearchCountRequest> validator
            ) =>
            {
                var req = new LaTotalSearchCountRequest(serviceTypeId, laOrgId);
                await validator.ValidateAndThrowAsync(req);
                return await getServiceSearchFactQuery.GetTotalSearchCountForLa(req);
            });
    }
}
