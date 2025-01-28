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
var servicesPath = configuration["FilePath"];
var seedOrgsPath = configuration["SeedOrgsFilePath"];

// Check that we have the required values
if (string.IsNullOrEmpty(servicesPath))
{
    throw new ArgumentNullException(nameof(servicesPath));
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new ArgumentNullException(nameof(connectionString));
}

if(string.IsNullOrEmpty(seedOrgsPath))
{
    throw new ArgumentNullException(nameof(seedOrgsPath));
}

// Add the DbContext
services.AddDbContext<DedsDbContext>(options =>
    options.UseSqlServer(connectionString, x => x.UseNetTopologySuite()));

// Add the services
services.AddTransient<IFileReaderService, FileReaderService>();
services.AddTransient<IUploadService, UploadService>();

var serviceProvider = services.BuildServiceProvider();

var fileService = serviceProvider.GetRequiredService<IFileReaderService>();
var uploadService = serviceProvider.GetRequiredService<IUploadService>();


// Seed the organisations
var orgsData = fileService.GetSeedOrganisationsFromCsv(seedOrgsPath);
await uploadService.SeedOrganisationsAsync(orgsData);

// Read the file to get services
var servicesData = fileService.GetServicesFromXlsx(servicesPath);
await uploadService.UploadServicesAsync(servicesData);

