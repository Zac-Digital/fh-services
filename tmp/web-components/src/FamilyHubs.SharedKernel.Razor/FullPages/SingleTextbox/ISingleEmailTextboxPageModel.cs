using System.ComponentModel.DataAnnotations;

namespace FamilyHubs.SharedKernel.Razor.FullPages.SingleTextbox;

public interface ISingleEmailTextboxPageModel : ISingleTextboxPageModel
{
    [EmailAddress]
    new string? TextBoxValue { get; set; }
}