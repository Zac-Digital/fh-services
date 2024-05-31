using FamilyHubs.SharedKernel.Razor.ErrorNext;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Immutable;

namespace FamilyHubs.Example.Pages.Examples.Errors;

public class ErrorNextModel : PageModel
{
    public enum ExampleErrors
    {
        Error1,
        Error2,
        Error3
    }

    public static readonly ImmutableDictionary<int, PossibleError> PossibleErrors =
        ImmutableDictionary.Create<int, PossibleError>()
            .Add(ExampleErrors.Error1, "Error 1 message")
            .Add(ExampleErrors.Error2, "Error 2 message")
            .Add(ExampleErrors.Error3, "Error 3 message");

    public IErrorState Errors { get; set; } = ErrorState.Empty;

    public string? Field1 { get; set; }
    public string? Field2 { get; set; }
    public string? Field3 { get; set; }

    public void OnGet()
    {
        Errors = ErrorState.Create(PossibleErrors, ExampleErrors.Error2, ExampleErrors.Error3 );
    }
}