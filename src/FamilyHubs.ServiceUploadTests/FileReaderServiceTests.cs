using FamilyHubs.ServiceUpload;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FamilyHubs.ServiceUploadTests
{
    public class FileReaderServiceTests
    {
        private readonly ILogger<FileReaderService> _loggerMock;
        private readonly FileReaderService _fileReaderService;

        public FileReaderServiceTests()
        {
            _loggerMock = Substitute.For<ILogger<FileReaderService>>();
            _fileReaderService = new FileReaderService(_loggerMock);
        }

        [Fact]
        public void GetDataFromCsv_ShouldThrowException_WhenFileIsNotCsv()
        {
            // Arrange
            const string fileName = "./data/notACsv.txt";

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _fileReaderService.GetServicesFromCsv(fileName));
            Assert.Equal("File is not a CSV", exception.Message);
        }

        [Fact]
        public void GetDataFromCsv_ShouldThrowException_WhenHeadersAreMissing()
        {
            // Arrange
            const string fileName = "./data/missingHeader.csv";

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _fileReaderService.GetServicesFromCsv(fileName));
            Assert.Equal("Missing Header: Id", exception.Message);
        }

        [Fact]
        public void GetDataFromCsv_ShouldReturnData_WhenFileIsValid()
        {
            // Arrange
            var fileName = "./data/goodData.csv";

            // Act
            var result = _fileReaderService.GetServicesFromCsv(fileName);

            // Assert
            Assert.Single(result);
            var data = result.First();
            Assert.Equal(Guid.Parse("47bdeab7-ad40-4df6-89b3-96f6e0bc906f"), data.Id);
            Assert.Equal("Test Service", data.Name);
            Assert.Equal("Test Service Description", data.Description);
            Assert.Equal("01234567890", data.Phone);
            Assert.Equal("test@test.com", data.Email);
            Assert.Equal("www.testexampletest.com", data.Url);
            Assert.Equal("Test Organisation", data.OrganisationName);
            Assert.Equal("Test La", data.La);
        }
    }
}