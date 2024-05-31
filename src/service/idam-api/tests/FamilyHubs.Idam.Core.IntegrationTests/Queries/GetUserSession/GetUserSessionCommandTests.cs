using AutoMapper;
using FamilyHubs.Idam.Core.Exceptions;
using FamilyHubs.Idam.Core.Models;
using FamilyHubs.Idam.Core.Queries.GetUserSession;
using FamilyHubs.Idam.Data.Entities;
using Moq;

namespace FamilyHubs.Idam.Core.IntegrationTests.Queries.GetUserSession
{
    public class GetUserSessionCommandTests : DataIntegrationTestBase<GetUserSessionCommandHandler>
    {
        private readonly Mock<IMapper> _mockMapper;
        private const string _expectedSid = "expectedSid";
        private const string _expectedEmail = "expected@email.com";

        public GetUserSessionCommandTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockMapper
                .Setup(m=>m.Map<UserSessionDto>(It.IsAny<UserSession>()))
                .Returns((UserSession source)=> new UserSessionDto { Email = source.Email, Sid = source.Sid});
        }

        [Fact]
        public async Task Handle_RecordNotFound_Throws()
        {
            //  Arrange
            var command = new GetUserSessionCommand { Sid = null, Email = null };
            var sut = new GetUserSessionCommandHandler(TestDbContext, MockLogger.Object, _mockMapper.Object);

            //  Act
            var exception = await Assert.ThrowsAsync<BadRequestException>(async () => await sut.Handle(command, new CancellationToken()));

            //  Assert
            Assert.Equal("Get UserSession must query by 'email' or 'sid'", exception.Message);

        }

        [Theory]
        [InlineData(_expectedSid, null)]
        [InlineData(null, _expectedEmail)]
        public async Task Handle_ReturnsRecord(string? sid, string? email)
        {
            //  Arrange
            var userSession = new UserSession { Sid = _expectedSid, Email = _expectedEmail};
            TestDbContext.Add(userSession);
            await TestDbContext.SaveChangesAsync();

            var command = new GetUserSessionCommand { Sid = sid, Email = email };
            var sut = new GetUserSessionCommandHandler(TestDbContext, MockLogger.Object, _mockMapper.Object);

            //  Act
            var result = await sut.Handle(command, new CancellationToken());

            //  Assert
            Assert.NotNull(result);
            Assert.Equal(userSession.Sid, result.Sid);
            Assert.Equal(userSession.Email, result.Email);

        }

    }
}
