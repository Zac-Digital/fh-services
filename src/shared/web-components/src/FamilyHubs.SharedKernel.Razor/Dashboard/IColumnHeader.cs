
namespace FamilyHubs.SharedKernel.Razor.Dashboard;

public interface IColumnHeader
{
    /// <summary>
    /// If null, the column will not be sortable.
    /// Otherwise, the columns will be sortable, and sorted according to the SortOrder.
    /// </summary>
    SortOrder? Sort { get; }

    /// <summary>
    /// This will be rendered as the column header's content. It can contain HTML.
    /// </summary>
    string ContentAsHtml { get; }

    /// <summary>
    /// The (space separated) classes to be applied to the header element.
    /// </summary>
    public string? HeaderClasses { get; }

    /// <summary>
    /// The (space separated) classes to be applied to the cell element.
    /// </summary>
    public string? CellClasses { get; }
}