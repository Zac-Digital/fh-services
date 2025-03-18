using System.Globalization;
using System.Security.Claims;
using FamilyHubs.Referral.Web.Pages.ProfessionalReferral;
using FamilyHubs.SharedKernel.Identity;
using FamilyHubs.SharedKernel.Services.Postcode.Interfaces;
using FamilyHubs.SharedKernel.Services.Postcode.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using NSubstitute;

namespace FamilyHubs.ReferralUi.UnitTests.Web.Pages.ProfessionalReferral;

public class WhenUsingSearch
{
    private readonly IPostcodeLookup _postcodeLookup;

    public WhenUsingSearch()
    {
        _postcodeLookup = Substitute.For<IPostcodeLookup>();
    }

    [Fact]
    public void OnGet_PublicUser_ShouldGet_NonLoggedInPage()
    {
        SearchModel searchModel = new SearchModel(_postcodeLookup)
        {
            PageContext = GetPageContext(null)
        };

        _ = searchModel.OnGet();

        searchModel.IsNotLoggedIn.Should().BeTrue();
    }

    [Fact]
    public void OnGet_PractitionerUser_ShouldGet_LoggedInPage()
    {
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim("role", RoleTypes.LaDualRole),
                new Claim(FamilyHubsClaimTypes.OrganisationId, "1"),
                new Claim(FamilyHubsClaimTypes.AccountId, "1"),
                new Claim(FamilyHubsClaimTypes.AccountStatus, "Active"),
                new Claim(FamilyHubsClaimTypes.FullName, "Jane Doe"),
                new Claim(FamilyHubsClaimTypes.ClaimsValidTillTime,
                    new DateTime().AddDays(1).ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.Email, "jane.doe@education.gov.uk"),
                new Claim(FamilyHubsClaimTypes.PhoneNumber, "01234567890"),
                new Claim(FamilyHubsClaimTypes.TermsAndConditionsAccepted, "Test")
            ]
        ));

        SearchModel searchModel = new SearchModel(_postcodeLookup)
        {
            PageContext = GetPageContext(claimsPrincipal)
        };

        _ = searchModel.OnGet();

        searchModel.IsNotLoggedIn.Should().BeFalse();
    }

    [Fact]
    public async Task OnPost_WhenPostcodeIsValid_ThenValidationShouldBeTrue()
    {
        //Arrange
        _postcodeLookup
            .Get(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((PostcodeError.None, null));
        var searchModel = new SearchModel(_postcodeLookup);

        //Act
        _ = await searchModel.OnPostAsync() as PageResult;

        //Assert
        Assert.False(searchModel.Errors.HasErrors);
    }

    [Fact]
    public async Task OnPost_WhenPostcodeIsNotValid_ThenValidationShouldBeFalse()
    {
        //Arrange
        const string postcode = "aaa";
        _postcodeLookup
            .Get(postcode, Arg.Any<CancellationToken>())
            .Returns((PostcodeError.InvalidPostcode, null));
        var searchModel = new SearchModel(_postcodeLookup)
        {
            Postcode = postcode
        };

        _ = await searchModel.OnPostAsync() as PageResult;

        //Assert
        Assert.True(searchModel.Errors.HasErrors);
    }
    
    private static PageContext GetPageContext(ClaimsPrincipal? claimsPrincipal)
    {
        HttpContext httpContext = new DefaultHttpContext();

        if (claimsPrincipal is not null)
        {
            httpContext.User = claimsPrincipal;
        }

        ModelStateDictionary modelState = new ModelStateDictionary();
        ActionContext actionContext =
            new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
        EmptyModelMetadataProvider modelMetadataProvider = new EmptyModelMetadataProvider();
        ViewDataDictionary viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
        PageContext pageContext = new PageContext(actionContext) { ViewData = viewData };

        return pageContext;
    }
}