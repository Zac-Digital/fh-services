using FamilyHubs.ServiceUpload;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Setup DI
var services = new ServiceCollection();
services.AddLogging(configure => configure.AddConsole());

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");
var fileName = configuration["FilePath"];

// Check that we have the required values
if (string.IsNullOrEmpty(fileName))
{
    throw new ArgumentNullException(nameof(fileName));
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new ArgumentNullException(nameof(connectionString));
}

// Add the DbContext
services.AddDbContext<DedsDbContext>(options =>
    options.UseSqlServer(connectionString, x => x.UseNetTopologySuite()));

// Add the services
services.AddTransient<IFileReaderService, FileReaderService>();
services.AddTransient<IUploadService, UploadService>();

var serviceProvider = services.BuildServiceProvider();

// Read the file to get services
var fileService = serviceProvider.GetRequiredService<IFileReaderService>();
var data = fileService.GetDataFromXlsx(fileName);

// Upload the services to the database
var uploadService = serviceProvider.GetRequiredService<IUploadService>();
await uploadService.UploadServicesAsync(data);