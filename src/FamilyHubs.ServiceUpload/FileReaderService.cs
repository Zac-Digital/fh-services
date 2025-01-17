using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceUpload;

public interface IFileReaderService
{
    MinimalDataDto[] GetDataFromCsv(string fileName);
}

public class FileReaderService : IFileReaderService
{
    private readonly ILogger<FileReaderService> _logger;

    public FileReaderService(ILogger<FileReaderService> logger)
    {
        _logger = logger;
    }
    
    public MinimalDataDto[] GetDataFromCsv(string fileName)
    {
        var fileExtension = Path.GetExtension(fileName);
        if (fileExtension != ".csv")
        {
            throw new Exception("File is not a CSV");
        }

        // Read the file
        var fileData = File.ReadAllLines(fileName);

        // Transform the data
        var headerRow = fileData[0].Split(',');
        _logger.LogInformation("Headers found: {HeaderCount}", headerRow.Length);
        
        var headerDictionary = headerRow
            .Select((header, index) => new { header, index })
            .ToDictionary(x => x.header, x => x.index, StringComparer.OrdinalIgnoreCase);
        
        const string idHeader = "Id";
        const string nameHeader = "Name";
        const string descriptionHeader = "Description";
        const string phoneHeader = "Phone";
        const string emailHeader = "Email";
        const string urlHeader = "Url";
        const string organisationNameHeader = "Organisation Name";
        const string laHeader = "La";


        var requiredHeaders = new[] { idHeader, nameHeader, descriptionHeader, phoneHeader, emailHeader, urlHeader, organisationNameHeader, laHeader };

        foreach (var header in requiredHeaders)
        {
            if (!headerDictionary.ContainsKey(header))
            {
                throw new Exception($"Missing Header: {header}");
            }
        }

        var data = fileData.Skip(1).Select(d =>
        {
            var columns = d.Split(',');
            return new MinimalDataDto
            {
                Id = Guid.Parse(columns[headerDictionary[idHeader]].Trim()),
                OrganisationName = columns[headerDictionary[organisationNameHeader]].Trim(),
                La = columns[headerDictionary[laHeader]].Trim(),
                Name = columns[headerDictionary[nameHeader]].Trim(),
                Description = columns[headerDictionary[descriptionHeader]].Trim(),
                Phone = columns[headerDictionary[phoneHeader]].Trim(),
                Email = columns[headerDictionary[emailHeader]].Trim(),
                Url = columns[headerDictionary[urlHeader]].Trim(),
            };
        }).ToArray();

        return data;
    }
}