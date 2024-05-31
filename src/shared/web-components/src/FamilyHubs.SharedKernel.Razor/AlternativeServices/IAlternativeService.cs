
namespace FamilyHubs.SharedKernel.Razor.AlternativeServices;

public interface IAlternativeService
{
    string? ServiceName { get; }
    // we could support alternative Layouts too, but we don't need it yet
}