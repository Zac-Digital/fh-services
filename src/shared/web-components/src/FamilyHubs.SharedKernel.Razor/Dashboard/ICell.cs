
namespace FamilyHubs.SharedKernel.Razor.Dashboard;

public interface ICell
{
    /// <summary>
    /// If a PartialName is given, that partial will be rendered instead of ContentAsHtml,
    /// with an instance of the IDashboard generic type as the model.
    /// </summary>
    string? PartialName { get; }

    /// <summary>
    /// If PartialName is null, this will be rendered as the cell's content. It can contain HTML.
    /// </summary>
    string? ContentAsHtml { get; }
}