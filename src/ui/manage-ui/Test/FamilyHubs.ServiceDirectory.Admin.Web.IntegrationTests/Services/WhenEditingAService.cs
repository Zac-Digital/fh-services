using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Xunit;

namespace FamilyHubs.ServiceDirectory.Admin.Web.IntegrationTests.Services;

public class WhenEditingAService : BaseServiceTest
{
    [Theory]
    [MemberData(nameof(Data))]
    public async Task BackWorksCorrectly(string rowSelector, string _)
    {
        await Login(StubUser.DfeAdmin);

        var page = await Navigate("manage-services/Service-Detail?flow=Edit");
        var changeLink = page.QuerySelector($"{rowSelector} a") as IHtmlAnchorElement;

        var changePage = await Navigate(changeLink!.Href);

        var firstBackLink = changePage.QuerySelector("a.govuk-back-link") as IHtmlAnchorElement;
        var firstBackPage = await Navigate(firstBackLink!.Href);

        var secondBackLink = firstBackPage.QuerySelector("a.govuk-back-link") as IHtmlAnchorElement;
        var secondBackPage = await Navigate(secondBackLink!.Href);

        var heading = secondBackPage.QuerySelector("h1");
        Assert.Equal("Services", heading.GetInnerText());
    }

    [Theory]
    [MemberData(nameof(Data))]
    public async Task CanGoBackAfterSaving(string rowSelector, string buttonSelector)
    {
        await Login(StubUser.DfeAdmin);

        var page = await Navigate("manage-services/Service-Detail?flow=Edit");
        var changeLink = page.QuerySelector($"{rowSelector} a") as IHtmlAnchorElement;

        var changePage = await Navigate(changeLink!.Href);

        // Save!
        var formButton = changePage.QuerySelector($"form {buttonSelector}") as IHtmlButtonElement;
        var submitPage = await SubmitForm(formButton!);

        var backLink = submitPage.QuerySelector("a.govuk-back-link") as IHtmlAnchorElement;
        var backPage = await Navigate(backLink!.Href);

        var heading = backPage.QuerySelector("h1");
        Assert.Equal("Services", heading.GetInnerText());
    }

    [Theory]
    [MemberData(nameof(CanFailData))]
    public async Task CanGoBackAfterFailingThenSaving(string rowSelector, string buttonSelector)
    {
        await Login(StubUser.DfeAdmin);

        var page = await Navigate("manage-services/Service-Detail?flow=Edit");
        var changeLink = page.QuerySelector($"{rowSelector} a") as IHtmlAnchorElement;

        var changePage = await Navigate(changeLink!.Href);

        // Fail first by submitting no values
        var formButton = changePage.QuerySelector($"form {buttonSelector}") as IHtmlButtonElement;
        var formValues = GenerateFormValues(formButton!);
        var failPage = await SubmitForm(formButton!.Form!.Action, formValues.Where(kv => kv.Key == "__RequestVerificationToken" || kv.Key == formButton.Name));

        // Save
        var failFormButton = failPage.QuerySelector($"form {buttonSelector}") as IHtmlButtonElement;
        var submitPage = await SubmitForm(failFormButton!);

        var backLink = submitPage.QuerySelector("a.govuk-back-link") as IHtmlAnchorElement;
        var backPage = await Navigate(backLink!.Href);

        var heading = backPage.QuerySelector("h1");
        Assert.Equal("Services", heading.GetInnerText());
    }

    private class TestCase(string rowName, string? testId = null, bool canFail = true)
    {
        public readonly bool CanFail = canFail;

        public string RowSelector => $"[data-testid=\"{rowName}-row\"]";

        public string ButtonSelector =>
            testId != null ? $"[data-testId=\"{testId}\"]" : "button[type=\"submit\"]";

        public override string ToString() => rowName;
    }

    private static readonly IEnumerable<TestCase> RawData =
    [
        new("la"),
        new("org"),
        new("name"),
        new("support"),
        new("description"),
        new("who"),
        new("lang", "continue-button"),
        new("cost"),
        new("how"),
        new("locations", "continue-button", false),
        new("days-1", canFail: false),
        new("extra-1"),
        new("days", canFail: false),
        new("extra"),
        new("contact"),
        new("more", canFail: false)
    ];

    public static TheoryData<string, string> Data() => FromEnumerable(RawData);
    public static TheoryData<string, string> CanFailData() => FromEnumerable(RawData.Where(test => test.CanFail));

    private static TheoryData<string, string> FromEnumerable(IEnumerable<TestCase> cases)
    {
        var data = new TheoryData<string, string>();
        foreach (var testCase in cases)
        {
            data.Add(testCase.RowSelector, testCase.ButtonSelector);
        }
        return data;
    }
}
