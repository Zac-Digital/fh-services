using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using FamilyHubs.Referral.Core.ApiClients;
using FamilyHubs.Referral.Web.Pages.ProfessionalReferral;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.Identity;
using FamilyHubs.SharedKernel.Razor.FeatureFlags;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.FeatureManagement;
using NSubstitute;

namespace FamilyHubs.ReferralUi.UnitTests.Web.Pages.ProfessionalReferral;

public class WhenUsingLocalOfferDetail
{
    private const string ServiceId = "1";

    private readonly IOrganisationClientService _organisationClientService;
    private readonly IIdamsClient _idamsClient;
    private readonly IFeatureManager _featureManager;

    private readonly LocalOfferDetailModel _localOfferDetailModel;

    public WhenUsingLocalOfferDetail()
    {
        _organisationClientService = Substitute.For<IOrganisationClientService>();
        _idamsClient = Substitute.For<IIdamsClient>();
        _featureManager = Substitute.For<IFeatureManager>();

        _featureManager.IsEnabledAsync(FeatureFlag.ConnectDashboard).Returns(true);
        _featureManager.IsEnabledAsync(FeatureFlag.VcfsServices).Returns(true);

        ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity([new Claim("role", RoleTypes.LaDualRole)]));

        HttpContext httpContext = new DefaultHttpContext { User = principal };
        ModelStateDictionary modelState = new ModelStateDictionary();
        ActionContext actionContext =
            new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
        EmptyModelMetadataProvider modelMetadataProvider = new EmptyModelMetadataProvider();
        ViewDataDictionary viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
        TempDataDictionary tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        PageContext pageContext = new PageContext(actionContext) { ViewData = viewData };

        _localOfferDetailModel = new LocalOfferDetailModel(_organisationClientService, _idamsClient, _featureManager)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        httpContext.Request.Headers.Referer = "Test";
    }
    
    [Fact]
    public async Task OnGetAsync_Returns_ServiceDetailPage()
    {
        ServiceDto serviceDto = new()
        {
            Name = "Service",
            ServiceType = ServiceType.InformationSharing,
            Schedules =
            [
                new ScheduleDto
                {
                    ServiceId = int.Parse(ServiceId),
                    AttendingType = AttendingType.Online.ToString()
                },
                new ScheduleDto
                {
                    ServiceId = int.Parse(ServiceId),
                    AttendingType = AttendingType.Telephone.ToString(),
                }
            ]
        };

        _organisationClientService.GetLocalOfferById(ServiceId).Returns(serviceDto);
        _idamsClient.GetVcsProfessionalsEmailsAsync(Arg.Any<long>()).Returns(["test@test.test"]);

        IActionResult result = await _localOfferDetailModel.OnGetAsync(ServiceId);

        result.Should().NotBeNull();

        _localOfferDetailModel.LocalOffer.Should().BeEquivalentTo(serviceDto);
        
        _localOfferDetailModel.ServiceScheduleAttendingTypes.Should().HaveCount(2);
        
        _localOfferDetailModel.ServiceSchedule.Should().NotBeNull();
        _localOfferDetailModel.ServiceSchedule!.ServiceId.Should().Be(int.Parse(ServiceId));
        _localOfferDetailModel.ServiceSchedule!.AttendingType.Should().Be(AttendingType.Online.ToString());

        _localOfferDetailModel.ReturnUrl.Should().Be("Test");

        _localOfferDetailModel.ServiceId.Should().Be(ServiceId);
        
        _localOfferDetailModel.ShowConnectionRequestButton.Should().BeTrue();
    }

    [Fact]
    public async Task
        OnGetAsync_Returns_ServiceDetailPage_With_ShowConnectionRequestsDisabled_When_FeatureFlag_ConnectDashboard_IsDisabled()
    {
        _featureManager.IsEnabledAsync(FeatureFlag.ConnectDashboard).Returns(false);
        
        ServiceDto serviceDto = new()
        {
            Name = "Service",
            ServiceType = ServiceType.InformationSharing
        };

        _organisationClientService.GetLocalOfferById(ServiceId).Returns(serviceDto);

        IActionResult result = await _localOfferDetailModel.OnGetAsync(ServiceId);

        result.Should().NotBeNull();
        
        _localOfferDetailModel.ShowConnectionRequestButton.Should().BeFalse();
    }
    
    [Fact]
    public async Task
        OnGetAsync_Returns_ServiceDetailPage_With_ShowConnectionRequestsDisabled_When_OrganisationHasNoVcsUsers()
    {
        _idamsClient.GetVcsProfessionalsEmailsAsync(Arg.Any<long>()).Returns([]);
        
        ServiceDto serviceDto = new()
        {
            Name = "Service",
            ServiceType = ServiceType.InformationSharing
        };

        _organisationClientService.GetLocalOfferById(ServiceId).Returns(serviceDto);

        IActionResult result = await _localOfferDetailModel.OnGetAsync(ServiceId);

        result.Should().NotBeNull();
        
        _localOfferDetailModel.ShowConnectionRequestButton.Should().BeFalse();
    }

    [Fact]
    public async Task OnGetAsync_Returns_ServiceDetailPage_When_FeatureFlag_VcfsServices_IsEnabled()
    {
        ServiceDto serviceDto = new()
        {
            Name = "Service",
            ServiceType = ServiceType.InformationSharing
        };

        _organisationClientService.GetLocalOfferById(ServiceId).Returns(serviceDto);

        IActionResult result = await _localOfferDetailModel.OnGetAsync(ServiceId);

        result.Should().NotBeNull();

        _localOfferDetailModel.LocalOffer.Should().BeEquivalentTo(serviceDto);
    }

    [Fact]
    public async Task OnGetAsync_Returns_404Error_When_FeatureFlag_VcfsServices_IsDisabled()
    {
        ServiceDto serviceDto = new()
        {
            Name = "Service",
            ServiceType = ServiceType.InformationSharing
        };

        _featureManager.IsEnabledAsync(FeatureFlag.VcfsServices).Returns(false);

        _organisationClientService.GetLocalOfferById(ServiceId).Returns(serviceDto);

        IActionResult result = await _localOfferDetailModel.OnGetAsync(ServiceId);

        result.Should().BeOfType<RedirectToPageResult>();
        RedirectToPageResult? redirectToPageResult = result as RedirectToPageResult;
        Assert.NotNull(redirectToPageResult);
        redirectToPageResult.PageName.Should().Be("/Error/404");
    }
}