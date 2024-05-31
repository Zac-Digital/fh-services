using FamilyHubs.SharedKernel.Razor.ErrorNext;

namespace FamilyHubs.SharedKernel.Razor.FullPages.SingleTextbox;

//todo: composable concrete?

public interface ISingleTextboxPageModel
{
    /// <summary>
    /// Optional separate heading. If not supplied, the heading will be the same as the <see cref="TextBoxLabel"/>.
    /// </summary>
    string? HeadingText { get; }
    string? HintText { get; }
    string TextBoxLabel { get; }
    string? TextBoxValue { get; }

    int? MaxLength { get; }

    IErrorState Errors { get; }
}