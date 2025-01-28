namespace FamilyHubs.ServiceUpload.Models;

public class MinimalDataDto
{
    public required Guid Id { get; init; }
    public required string OrganisationName { get; init; }
    public required string La { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Url { get; init; }
}