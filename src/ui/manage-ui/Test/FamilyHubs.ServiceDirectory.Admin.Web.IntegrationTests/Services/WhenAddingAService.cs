using System.Text;
using System.Text.Json;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FamilyHubs.ServiceDirectory.Admin.Core.ApiClient;
using FamilyHubs.ServiceDirectory.Admin.Core.Models;
using FamilyHubs.ServiceDirectory.Admin.Core.Models.ServiceJourney;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Models;
using FamilyHubs.SharedKernel.Razor.Dashboard;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace FamilyHubs.ServiceDirectory.Admin.Web.IntegrationTests.Services;

public class WhenAddingAService : BaseServiceTest
{
    [Fact]
    public async Task BackWorksCorrectly()
    {
        await Login(StubUser.DfeAdmin);

        var page = await Navigate("manage-services/Service-Detail?flow=Add");

        for (var i = 0; i < 17; i++)
        {
            var backLink = page.QuerySelector("a.govuk-back-link") as IHtmlAnchorElement;
            page = await Navigate(backLink!.Href);
        }

        var heading = page.QuerySelector("h1");
        Assert.Equal("Services", heading.GetInnerText());
    }
}
