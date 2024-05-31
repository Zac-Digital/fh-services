using FamilyHubs.SharedKernel.Razor.Dashboard;
using FamilyHubs.SharedKernel.Razor.Pagination;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyHubs.Example.Pages.Examples.Dashboard;

public record RowData(string Foo, string Bar);

public class Row : IRow<RowData>
{
    public RowData Item { get; }

    public Row(RowData data)
    {
        Item = data;
    }

    public IEnumerable<ICell> Cells
    {
        get
        {
            yield return new Cell(Item.Foo);
            yield return new Cell(Item.Bar);
            yield return new Cell($"{Item.Foo} + {Item.Bar}");
            yield return new Cell("100");
        }
    }
}

public class IndexModel : PageModel, IDashboard<RowData>
{
    private enum Column
    {
        SortableColumn1,
        SortableColumn2
    }

    private static ColumnImmutable[] _columnImmutables =
    {
        new("Sortable column 1", Column.SortableColumn1.ToString()),
        new("No sort"),
        new("Sortable column 2", Column.SortableColumn2.ToString(), ColumnType.AlignedRight),
        new("Numeric", ColumnType: ColumnType.Numeric)
    };

    private IEnumerable<IColumnHeader> _columnHeaders = Enumerable.Empty<IColumnHeader>();
    private IEnumerable<IRow<RowData>> _rows = Enumerable.Empty<IRow<RowData>>();

    IEnumerable<IColumnHeader> IDashboard<RowData>.ColumnHeaders => _columnHeaders;
    IEnumerable<IRow<RowData>> IDashboard<RowData>.Rows => _rows;

    public void OnGet(string? columnName, SortOrder sort)
    {
        if (columnName == null || !Enum.TryParse(columnName, true, out Column column))
        {
            // default when first load the page, or user has manually changed the url
            column = Column.SortableColumn1;
            sort = SortOrder.ascending;
        }

        _columnHeaders = new ColumnHeaderFactory(_columnImmutables, "/Examples/Dashboard", column.ToString(), sort)
            .CreateAll();

        _rows = GetSortedRows(column, sort);
    }

    string? IDashboard<RowData>.TableClass => "app-dashboard-class";

    public IPagination Pagination
    {
        get => ILinkPagination.DontShow;
        set => throw new NotImplementedException();
    }

    private IEnumerable<Row> GetSortedRows(Column column, SortOrder sort)
    {
        if (sort == SortOrder.ascending)
        {
            return GetExampleData().OrderBy(r => GetValue(column, r));
        }

        return GetExampleData().OrderByDescending(r => GetValue(column, r));
    }

    private static string GetValue(Column column, Row r)
    {
        return column switch
        {
            Column.SortableColumn1 => r.Item.Foo,
            Column.SortableColumn2 => r.Item.Bar,
            _ => throw new InvalidOperationException($"Unknown column: {column}")
        };
    }

    private Row[] GetExampleData()
    {
        return new Row[]
        {
            new(new RowData("foo", "bar")),
            new(new RowData("123", "456")),
            new(new RowData("alice", "bob")),
            new(new RowData("Bar", "Foo"))
        };
    }
}