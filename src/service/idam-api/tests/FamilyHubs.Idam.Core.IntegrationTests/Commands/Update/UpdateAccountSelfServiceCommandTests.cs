using FamilyHubs.Idam.Core.Commands.Update;
using FamilyHubs.Idam.Core.Exceptions;
using FamilyHubs.Idam.Data.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace FamilyHubs.Idam.Core.IntegrationTests.Commands.Update;

public class UpdateAccountSelfServiceCommandTests : DataIntegrationTestBase<UpdateAccountSelfServiceCommandHandler>
{
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

    public UpdateAccountSelfServiceCommandTests()
    {
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var claims = new[]
        {
            new Claim("AccountId", "1"), // Set the bearer token ID to 1
        };

        _mockHttpContextAccessor.Setup(mock => mock.HttpContext!.User.Claims).Returns(claims);
    }

    [Fact]
    public async Task ThenAnAccountIsUpdated()
    {
        const long accountId = 1;
        const string newName = "New Name";

        //Arrange
        var command = new UpdateAccountSelfServiceCommand
        {
            AccountId = accountId,
            Name = newName
        };

        var commandHandler = new UpdateAccountSelfServiceCommandHandler(
            TestDbContext,
            _mockHttpContextAccessor.Object,
            MockLogger.Object);

        //Act
        await commandHandler.Handle(command, new CancellationToken());

        //Assert
        TestDbContext.Accounts.Should().ContainSingle(a => a.Id == accountId && a.Name == newName);
    }

    [Fact]
    public async Task ThenUpdatingSomeoneElsesAccountThrowsException()
    {
        //Arrange
        const long accountId = 2;

        TestDbContext.Accounts.Add(new Account
        {
            Id = accountId,
            Name = "Test User2",
            Email = "original@example.com",
            PhoneNumber = "1234567890",
            Status = 0,
            Claims = new List<AccountClaim>()
        });
        await TestDbContext.SaveChangesAsync();

        var command = new UpdateAccountSelfServiceCommand()
        {
            AccountId = accountId,
            Name = "Not used"
        };

        var commandHandler = new UpdateAccountSelfServiceCommandHandler(
            TestDbContext,
            _mockHttpContextAccessor.Object,
            MockLogger.Object);

        //Act
        var result = async () => await commandHandler.Handle(command, new CancellationToken());

        //Assert
        await result.Should().ThrowAsync<AuthorisationException>();
    }

    [Fact]
    public async Task ThenUpdatingAnAccountThatDoesNotExistThrowsNotFoundException()
    {
        //Arrange
        var command = new UpdateAccountSelfServiceCommand()
        {
            AccountId = -1,
            Name = "Not used"
        };

        var commandHandler = new UpdateAccountSelfServiceCommandHandler(
            TestDbContext,
            _mockHttpContextAccessor.Object,
            MockLogger.Object);

        //Act
        var result = async () => await commandHandler.Handle(command, new CancellationToken());

        //Assert
        await result.Should().ThrowAsync<Ardalis.GuardClauses.NotFoundException>();
    }
}
