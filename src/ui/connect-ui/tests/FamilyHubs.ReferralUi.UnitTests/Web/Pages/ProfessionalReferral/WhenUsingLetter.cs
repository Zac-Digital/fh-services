﻿using FamilyHubs.Referral.Web.Pages.ProfessionalReferral;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FamilyHubs.ReferralUi.UnitTests.Web.Pages.ProfessionalReferral;

public class WhenUsingLetter : BaseProfessionalReferralPage
{
    private readonly LetterModel _letterModel;

    public WhenUsingLetter()
    {
        _letterModel = new LetterModel(ReferralDistributedCache);
    }

    [Fact]
    public async Task ThenOnGetLetter()
    {
        // Act
        await _letterModel.OnGetAsync("1");

        _letterModel.AddressLine1.Should().Be(AddressLine1);
        _letterModel.AddressLine2.Should().Be(AddressLine2);
        _letterModel.TownOrCity.Should().Be(TownOrCity);
        _letterModel.County.Should().Be(County);
        _letterModel.Postcode.Should().Be(Postcode);
    }

    [Fact]
    public async Task ThenOnPostLetter_RedirectToContactMethods()
    {
        //Act
        var result = await _letterModel.OnPostAsync("1") as RedirectToPageResult;

        result.Should().NotBeNull();
        result!.PageName.Should().Be("/ProfessionalReferral/ContactMethods");
    }

    [Fact]
    public async Task ThenOnPostWithValidationError()
    {
        _letterModel.ModelState.SetModelValue("AddressLine1", new ValueProviderResult("al1"));
        _letterModel.ModelState.SetModelValue("TownOrCity", new ValueProviderResult("toc"));
        _letterModel.ModelState.AddModelError("Postcode", "Enter a real postcode.");

        //Act
        await _letterModel.OnPostAsync("1");

        _letterModel.Errors.HasErrors.Should().BeTrue();
    }

    [Fact]
    public async Task ThenTheLetterBackUrl()
    {
        //Act
        await _letterModel.OnPostAsync("1");

        _letterModel.BackUrl.Should().Be("/ProfessionalReferral/Text?ServiceId=1");
    }
}
