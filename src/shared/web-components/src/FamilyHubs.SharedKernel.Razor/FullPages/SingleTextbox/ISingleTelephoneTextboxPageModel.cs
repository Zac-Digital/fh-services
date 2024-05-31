using System.ComponentModel.DataAnnotations;

namespace FamilyHubs.SharedKernel.Razor.FullPages.SingleTextbox
{
    public interface ISingleTelephoneTextboxPageModel : ISingleTextboxPageModel
    {
        [Phone]
        new string? TextBoxValue { get; set; }
    }
}