using FamilyHubs.SharedKernel.Razor.ErrorNext;
using FamilyHubs.SharedKernel.Razor.FullPages.Radios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Immutable;

namespace FamilyHubs.Example.Pages.Examples.FullPages.Radios;

public class RadioInlineModel : PageModel, IRadiosPageModel
{
    public static Radio[] StaticRadios => new[]
    {
        new Radio("England", Country.England.ToString()),
        new Radio("Scotland", Country.Scotland.ToString()),
        new Radio("Wales", Country.Wales.ToString()),
        new Radio("Northern Ireland", Country.NorthernIreland.ToString())
    };

    public IEnumerable<IRadio> Radios => StaticRadios;

    [BindProperty]
    public string? SelectedValue { get; set; }

    public IErrorState Errors { get; set; } = ErrorState.Empty;

    public string? Legend => "Where do you live?";

    public bool AreRadiosInline => true;

    public Country? SelectedCountry;

    public void OnGet()
    {
        // Do not pre-select radio options as this makes it more likely that users will:
        // * not realise they've missed a question
        // * submit the wrong answer

        // only preselect a radio button after the user has previously selected it
        //SelectedValue = Country.Wales.ToString();
    }

    public void OnPost()
    {
        if (SelectedValue == null)
        {
            Errors = ErrorState.Create(PossibleErrors, ErrorId.NoCountrySelected);
            return;
        }

        SelectedCountry = (Country)Enum.Parse(typeof(Country), SelectedValue);
    }

    public enum ErrorId
    {
        NoCountrySelected
    }

    public static readonly ImmutableDictionary<int, PossibleError> PossibleErrors =
        ImmutableDictionary.Create<int, PossibleError>()
            .Add(ErrorId.NoCountrySelected, "Select the country where you live");
}