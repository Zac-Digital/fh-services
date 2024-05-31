using System.Collections.Immutable;
using FamilyHubs.SharedKernel.Razor.ErrorNext;
using FamilyHubs.SharedKernel.Razor.FullPages.SingleTextbox;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyHubs.Example.Pages.Examples.FullPages.SingleTextbox;

/// <summary>
/// Simple example, not production ready, no P/R/G, loading existing value, retaining value etc.
/// </summary>
public class SingleTextboxModel : PageModel, ISingleTextboxPageModel
{
    public string? HeadingText => default;
    public string? HintText => default;
    public string TextBoxLabel => "What is the service name?";
    public int? MaxLength => 10;
    public IErrorState Errors { get; private set; } = ErrorState.Empty;

    [BindProperty]
    public string? TextBoxValue { get; set; }

    public void OnGet()
    {
    }

    public void OnPost()
    {
        if (string.IsNullOrWhiteSpace(TextBoxValue))
        {
            Errors = ErrorState.Create(PossibleErrors, ErrorId.AnswerMissing);
        }
    }

    public enum ErrorId
    {
        AnswerMissing
    }

    public static readonly ImmutableDictionary<int, PossibleError> PossibleErrors =
        ImmutableDictionary.Create<int, PossibleError>()
            .Add(ErrorId.AnswerMissing, "Guru meditation required");
}