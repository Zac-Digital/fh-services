using FamilyHubs.Idam.Core.Commands;
using FamilyHubs.Idam.Data.Entities;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

#if USE_EVENT_GRID

namespace FamilyHubs.Idam.Core.IntegrationTests.Commands
{

    public class SendEventGridMessageCommandTests : DataIntegrationTestBase<SendEventGridMessageCommandHandler>
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsContent()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(x => x["EventGridUrl"]).Returns("http://example.com/eventgrid");
            configurationMock.SetupGet(x => x["aeg-sas-key"]).Returns("dummy-key");

            var handler = new SendEventGridMessageCommandHandler(configurationMock.Object, MockLogger.Object);
            handler.IsUnitTesting = true;

            var account = new Account
            {
                Id = 1,
                Email = "test@example.com",
                Name = "Test User",
                PhoneNumber = "123-456-7890",
                Status = AccountStatus.Active,
                Claims = new[]
                {
                    new AccountClaim { AccountId = 1, Name = "OrganisationId", Value = "12345" }
                }
            };

            var request = new SendEventGridMessageCommand(account);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Handle_MissingEventGridUrl_ThrowsArgumentException()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(x => x["EventGridUrl"]).Returns(default(string));
            configurationMock.SetupGet(x => x["aeg-sas-key"]).Returns("dummy-key");

            var handler = new SendEventGridMessageCommandHandler(configurationMock.Object, MockLogger.Object);
            handler.IsUnitTesting = true;

            var account = new Account
            {
                Id = 1,
                Email = "test@example.com",
                Name = "Test User",
                PhoneNumber = "123-456-7890",
                Status = AccountStatus.Active,
                Claims = new[]
                {
                    new AccountClaim { AccountId = 1, Name = "OrganisationId", Value = "12345" }
                }
            };

            var request = new SendEventGridMessageCommand(account);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_MissingAegSasKey_ThrowsArgumentException()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(x => x["EventGridUrl"]).Returns("http://example.com/eventgrid");
            configurationMock.SetupGet(x => x["aeg-sas-key"]).Returns(default(string));

            var handler = new SendEventGridMessageCommandHandler(configurationMock.Object, MockLogger.Object);
            handler.IsUnitTesting = true;

            var account = new Account
            {
                Id = 1,
                Email = "test@example.com",
                Name = "Test User",
                PhoneNumber = "123-456-7890",
                Status = AccountStatus.Active,
                Claims = new[]
                {
                new AccountClaim { AccountId = 1, Name = "OrganisationId", Value = "12345" }
            }
            };

            var request = new SendEventGridMessageCommand(account);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}

#endif
