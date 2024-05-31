using FamilyHubs.SharedKernel.Razor.ErrorNext;
using FamilyHubs.SharedKernel.Razor.FullPages.SingleAutocomplete;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Immutable;

namespace FamilyHubs.Example.Pages.Examples.FullPages.SingleAutocomplete;

public class SingleAutocomplete_CustomModel : PageModel, ISingleAutocompletePageModel
{
    public string? ContentTop => "SingleAutocomplete-Custom-ContentTop";
    public string? ContentBottom => "SingleAutocomplete-Custom-ContentBottom";
    public IReadOnlyDictionary<string, HtmlString>? ContentSubstitutions { get; set; }

    [BindProperty]
    public string? SelectedValue { get; set; }
    public string Label => "Search and select the local authority area this service is in";
    public string? DisabledOptionValue => (-1).ToString();
    public IEnumerable<ISingleAutocompleteOption> Options { get; private set; } = Enumerable.Empty<ISingleAutocompleteOption>();
    public IErrorState Errors { get; private set; } = ErrorState.Empty;

    private static readonly IEnumerable<Dto> Dtos = new Dto[]
    {
        new(1, "First"),
        new(2, "Second"),
        new(3, "Third"),
    };

    public void OnGet()
    {
        Options = GetOptions();
        ContentSubstitutions = GetSubstitutions();

        // to preselect an option...
        //SelectedValue = Options.Skip(1).First().Value;
    }

    public IEnumerable<ISingleAutocompleteOption> GetOptions()
    {
        return Dtos.Select(x => new SingleAutocompleteOption(x.Id.ToString(), x.Name));
    }

    public IReadOnlyDictionary<string, HtmlString> GetSubstitutions()
    {
        return new Dictionary<string, HtmlString>
        {
            { "Label", new HtmlString("Where will Wales finish in the next Rugby World Cup?") },
            { "Substitution", new HtmlString("<h2>Substitution</h2>") }
        };
    }

    public void OnPost()
    {
        Options = GetOptions();
        ContentSubstitutions = GetSubstitutions();

        if (SelectedValue == null)
        {
            Errors = ErrorState.Create(PossibleErrors, ErrorId.NothingSelected);
            return;
        }

        long selectedId = long.Parse(SelectedValue);
        SelectedValue = Dtos.Single(d => d.Id == selectedId).Name;
    }

    public enum ErrorId
    {
        NothingSelected
    }

    public static readonly ImmutableDictionary<int, PossibleError> PossibleErrors =
        ImmutableDictionary.Create<int, PossibleError>()
            .Add(ErrorId.NothingSelected, "Enter something");
}