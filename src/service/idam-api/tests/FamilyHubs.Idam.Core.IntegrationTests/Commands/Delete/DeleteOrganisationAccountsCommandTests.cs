using FamilyHubs.Idam.Core.Commands.Delete;
using FluentAssertions;

namespace FamilyHubs.Idam.Core.IntegrationTests.Commands.Delete;

public class DeleteOrganisationAccountsCommandTests : DataIntegrationTestBase<DeleteOrganisationAccountsCommandHandler>
{
    [Fact]
    public async Task ReturnTrueWhenNoAccountClaimsPrenent()
    {
        //Arrange
        var command = new DeleteOrganisationAccountsCommand
        {
            OrganisationId = 1
        };

        var handler = new DeleteOrganisationAccountsCommandHandler(TestDbContext, MockLogger.Object);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().Be(true);
    }

    [Fact]
    public async Task ThenDeleteAccount()
    {
        //Arrange
        await CreateAccountClaim(TestSingleAccountClaim);
        var command = new DeleteOrganisationAccountsCommand
        {
            OrganisationId = 1
        };

        var handler = new DeleteOrganisationAccountsCommandHandler(TestDbContext, MockLogger.Object);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().Be(true);
    }

}

