using FamilyHubs.SharedKernel.Razor.ErrorNext;

namespace FamilyHubs.SharedKernel.Razor.FullPages.Checkboxes;

public interface ICheckboxesPageModel
{
    IEnumerable<ICheckbox> Checkboxes { get; }

    IEnumerable<string> SelectedValues { get; }

    IErrorState Errors { get; }

    string? DescriptionPartial { get; }
    string? Legend { get; }
    string? Hint { get; }
}