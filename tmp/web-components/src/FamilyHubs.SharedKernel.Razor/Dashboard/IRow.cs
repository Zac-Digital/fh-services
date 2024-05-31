
namespace FamilyHubs.SharedKernel.Razor.Dashboard;

public interface IRow<out T>
{
    /// <summary>
    /// The cells to be rendered.
    /// </summary>
    IEnumerable<ICell> Cells { get; }

    /// <summary>
    /// The item that the row is showing data for.
    /// </summary>
    T Item { get; }
}