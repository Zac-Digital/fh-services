using FamilyHubs.SharedKernel.Razor.Pagination;

namespace FamilyHubs.SharedKernel.Razor.Dashboard
{
    public interface IDashboard<out T>
    {
        /// <summary>
        /// A class (or classes) to be applied to the table element. Useful to target the table instance with CSS.
        /// </summary>
        string? TableClass { get; }

        /// <summary>
        /// The column headers to be rendered.
        /// </summary>
        IEnumerable<IColumnHeader> ColumnHeaders => Enumerable.Empty<IColumnHeader>();

        /// <summary>
        /// The rows to be rendered.
        /// </summary>
        IEnumerable<IRow<T>> Rows => Enumerable.Empty<IRow<T>>();

        /// <summary>
        /// The pagination component.
        /// </summary>
        IPagination Pagination { get; set; }
    }
}
