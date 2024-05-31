
namespace FamilyHubs.SharedKernel.Razor.Dashboard;

public record Cell(string? ContentAsHtml, string? PartialName = null) : ICell;