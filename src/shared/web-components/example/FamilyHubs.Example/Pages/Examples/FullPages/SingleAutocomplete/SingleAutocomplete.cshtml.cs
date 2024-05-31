using FamilyHubs.SharedKernel.Razor.ErrorNext;
using FamilyHubs.SharedKernel.Razor.FullPages.SingleAutocomplete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Immutable;

namespace FamilyHubs.Example.Pages.Examples.FullPages.SingleAutocomplete;

public record Dto(long Id, string Name);

public class SingleAutocompleteModel : PageModel, ISingleAutocompletePageModel
{
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
        Options = Dtos.Select(x => new SingleAutocompleteOption(x.Id.ToString(), x.Name));

        // to preselect an option...
        //SelectedValue = Options.Skip(1).First().Value;
    }

    public void OnPost()
    {
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