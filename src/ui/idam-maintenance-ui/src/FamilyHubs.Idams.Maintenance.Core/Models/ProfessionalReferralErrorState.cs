namespace FamilyHubs.Idams.Maintenance.Core.Models
{
    public record ProfessionalReferralErrorState(
    ConnectJourneyPage ErrorPage,
    ErrorId[] Errors,
    string[]? InvalidUserInput);
}
