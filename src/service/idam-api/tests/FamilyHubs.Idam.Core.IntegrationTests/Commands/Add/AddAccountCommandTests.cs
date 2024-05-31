using AutoFixture;
using FamilyHubs.Idam.Core.Commands.Add;
using FamilyHubs.Idam.Core.Exceptions;
using FamilyHubs.Idam.Data.Entities;
using FluentAssertions;

namespace FamilyHubs.Idam.Core.IntegrationTests.Commands.Add
{
    public class AddAccountCommandTests : DataIntegrationTestBase<AddAccountCommandHandler>
    {
        [Fact]
        public async Task Handle_AccountIsCreated()
        {
            //Arrange
            int sendCallBack = 0;
            var mockSender = GetMockEventGridSender("Done", () => sendCallBack++);
            var command = Fixture.Create<AddAccountCommand>();
            var commandHandler = new AddAccountCommandHandler(TestDbContext, mockSender.Object, MockLogger.Object);

            //Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            //Assert
            result.Should().Be(command.Email.ToLower());
#if USE_EVENT_GRID
            sendCallBack.Should().Be(1);
#endif
        }

        [Fact]
        public async Task Handle_RecordExists_ThrowsAlreadyExistException()
        {
            //Arrange
            int sendCallBack = 0;
            var mockSender = GetMockEventGridSender("Done", () => sendCallBack++);
            var existingAccount = Fixture.Create<Account>();
            TestDbContext.Accounts.Add(existingAccount);
            TestDbContext.SaveChanges();

            var command = new AddAccountCommand
            {
                Name = existingAccount.Name,
                Email = existingAccount.Email,
                PhoneNumber = existingAccount.PhoneNumber,
                Claims = existingAccount.Claims
            };

            var commandHandler = new AddAccountCommandHandler(TestDbContext, mockSender.Object, MockLogger.Object);

            //Act and Assert
            await Assert.ThrowsAsync<AlreadyExistsException>(() => commandHandler.Handle(command, new CancellationToken()));
            sendCallBack.Should().Be(0);
        }

#if USE_EVENT_GRID
        [Fact]
        public async Task Handle_EventGridFunctionThrowsException()
        {
            //Arrange
            int sendCallBack = 0;
            var mockSender = GetMockEventGridSender(new ArgumentException("EventGridUrl is missing"), () => sendCallBack++);
            var command = Fixture.Create<AddAccountCommand>();
            var commandHandler = new AddAccountCommandHandler(TestDbContext, mockSender.Object, MockLogger.Object);

            //Act
            var result = async () => await commandHandler.Handle(command, new CancellationToken());

            //Assert
            await result.Should().ThrowAsync<ArgumentException>();
            sendCallBack.Should().Be(1);
        }
#endif
    }
}
