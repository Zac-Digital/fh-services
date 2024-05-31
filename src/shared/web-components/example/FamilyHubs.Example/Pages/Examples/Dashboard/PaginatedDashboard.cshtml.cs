using FamilyHubs.SharedKernel.Razor.Dashboard;
using FamilyHubs.SharedKernel.Razor.Pagination;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyHubs.Example.Pages.Examples.Dashboard;

public class PaginatedDashboardModel : PageModel, IDashboard<RowData>
{
    private enum Column
    {
        Foo,
        Bar
    }

    private static ColumnImmutable[] _columnImmutables =
    {
        new("Foo", Column.Foo.ToString()),
        new("Bar", Column.Bar.ToString()),
        new("No sort (right align)", ColumnType: ColumnType.AlignedRight)
    };

    private IEnumerable<IColumnHeader> _columnHeaders = Enumerable.Empty<IColumnHeader>();
    private IEnumerable<IRow<RowData>> _rows = Enumerable.Empty<IRow<RowData>>();

    IEnumerable<IColumnHeader> IDashboard<RowData>.ColumnHeaders => _columnHeaders;
    IEnumerable<IRow<RowData>> IDashboard<RowData>.Rows => _rows;

    public IPagination Pagination { get; set; } = ILinkPagination.DontShow;

    string? IDashboard<RowData>.TableClass => "app-dashboard-class";

    public void OnGet(string? columnName, SortOrder sort, int currentPage = 1)
    {
        if (columnName == null || !Enum.TryParse(columnName, true, out Column column))
        {
            // default when first load the page, or user has manually changed the url
            column = Column.Foo;
            sort = SortOrder.ascending;
        }

        // only needed if e.g. the dashboard is showing content as decided by filters
        const string extraSearchTerms = "filter1=xyz&filter2=123";

        _columnHeaders = new ColumnHeaderFactory(_columnImmutables, "/Examples/Dashboard/PaginatedDashboard", column.ToString(), sort, extraSearchTerms)
            .CreateAll();

        const int pageSize = 10;
        _rows = GetSortedRows(column, sort, currentPage, pageSize);

        Pagination = new LargeSetLinkPagination<Column>(
            "/Examples/Dashboard/PaginatedDashboard",
            10, currentPage, column, sort, extraSearchTerms);
    }

    private static Row[] GetSortedRows(Column column, SortOrder sort, int page, int pageSize)
    {
        var data = Enumerable.Range(1, 100)
            .Select(i => new Row(new RowData($"foo {i:D3}", $"bar {101-i:D3}")));

        if (sort == SortOrder.ascending)
        {
            data = data.OrderBy(r => GetValue(column, r));
        }
        else
        {
            data = data.OrderByDescending(r => GetValue(column, r));
        }

        return data.Skip((page-1) * pageSize).Take(pageSize).ToArray();
    }

    private static string GetValue(Column column, Row r)
    {
        return column switch
        {
            Column.Foo => r.Item.Foo,
            Column.Bar => r.Item.Bar,
            _ => throw new InvalidOperationException($"Unknown column: {column}")
        };
    }
}