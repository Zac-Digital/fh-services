using FamilyHubs.ServiceUpload;
using FamilyHubs.ServiceUpload.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FamilyHubs.ServiceUploadTests;

public class UploadServiceTests
{
    private readonly DedsDbContext _context;
    private readonly UploadService _uploadService;

    public UploadServiceTests()
    {
        ILogger<UploadService> loggerMock = Substitute.For<ILogger<UploadService>>();

        var options = new DbContextOptionsBuilder<DedsDbContext>()
            .ConfigureWarnings(warning =>
            {
                warning.Ignore(InMemoryEventId.TransactionIgnoredWarning);
            })
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new DedsDbContext(options);
        _uploadService = new UploadService(loggerMock, _context);
    }

    [Fact]
    public async Task ShouldAddService_WhenValidDtoIsPassed()
    {
        // Arrange
        var servicesChangeDtos = new[]
        {
            new MinimalDataDto
            {
                Id = Guid.NewGuid(),
                Name = "Test Service",
                Description = "Test Description",
                OrganisationName = "Test Organisation",
                Phone = "1234567890",
                Email = "test@example.com",
                Url = "http://example.com",
                La = "Test La"
            }
        };

        // Act
        await _uploadService.UploadServicesAsync(servicesChangeDtos);

        // Assert
        var service = await _context.ServicesDbSet.FirstOrDefaultAsync();
        var organization = await _context.OrganizationsDbSet.FirstOrDefaultAsync();
        Assert.NotNull(service);
        Assert.Equal("Test Service", service.Name);
        Assert.Equal("Test Description", service.Description);
        Assert.Equal("1234567890", service.Contacts.First().Phones.First().Number);
        Assert.Equal("http://example.com", service.Url);
        Assert.Equal("test@example.com", service.Email);

        Assert.NotNull(organization);
        Assert.Equal("Test Organisation", organization.Name);
    }
    
    [Fact]
    public async Task ShouldAddOrganisation_WhenOneDoesNotExist()
    {
        // Arrange
        var servicesChangeDtos = new[]
        {
            new MinimalDataDto
            {
                Id = Guid.NewGuid(),
                Name = "Test Service",
                Description = "Test Description",
                OrganisationName = "Test Organisation",
                Phone = "1234567890",
                Email = "test@example.com",
                Url = "http://example.com",
                La = "Test La"
            }
        };

        // Act
        await _uploadService.UploadServicesAsync(servicesChangeDtos);

        // Assert
        var organization = await _context.OrganizationsDbSet.FirstOrDefaultAsync();

        Assert.NotNull(organization);
        Assert.Equal("Test Organisation", organization.Name);
        
    }
}