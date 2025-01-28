using ClosedXML.Excel;
using FamilyHubs.ServiceUpload.Models;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceUpload;

public interface IFileReaderService
{
    MinimalDataDto[] GetServicesFromCsv(string fileName);
    MinimalDataDto[] GetServicesFromXlsx(string fileName);
    
    OrgSeedDataDto[] GetSeedOrganisationsFromCsv(string fileName);
}

public class FileReaderService : IFileReaderService
{
    private readonly ILogger<FileReaderService> _logger;

    private const string IdHeader = "Id";
    private const string NameHeader = "Name";
    private const string DescriptionHeader = "Description";
    private const string PhoneHeader = "Phone";
    private const string EmailHeader = "Email";
    private const string UrlHeader = "Url";
    private const string OrganisationNameHeader = "Organisation Name";
    private const string LaHeader = "La";

    public FileReaderService(ILogger<FileReaderService> logger)
    {
        _logger = logger;
    }
    
    public MinimalDataDto[] GetServicesFromCsv(string fileName)
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


        var requiredHeaders = new[] { IdHeader, NameHeader, DescriptionHeader, PhoneHeader, EmailHeader, UrlHeader, OrganisationNameHeader, LaHeader };

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
                Id = Guid.Parse(columns[headerDictionary[IdHeader]].Trim()),
                OrganisationName = columns[headerDictionary[OrganisationNameHeader]].Trim(),
                La = columns[headerDictionary[LaHeader]].Trim(),
                Name = columns[headerDictionary[NameHeader]].Trim(),
                Description = columns[headerDictionary[DescriptionHeader]].Trim(),
                Phone = columns[headerDictionary[PhoneHeader]].Trim(),
                Email = columns[headerDictionary[EmailHeader]].Trim(),
                Url = columns[headerDictionary[UrlHeader]].Trim(),
            };
        }).ToArray();

        return data;
    }

    public MinimalDataDto[] GetServicesFromXlsx(string fileName)
    {
        // check file is xlsx
        var fileExtension = Path.GetExtension(fileName);
        if (fileExtension != ".xlsx")
        {
            throw new Exception("File is not an XLSX");
        }

        var requiredHeaders = new[] { IdHeader, NameHeader, DescriptionHeader, PhoneHeader, EmailHeader, UrlHeader, OrganisationNameHeader, LaHeader };
        using var workbook = new XLWorkbook(fileName);
        var worksheet = workbook.Worksheet(1);
            
        // check first row is headers and correct number
        var headerRow = worksheet.Row(1).Cells().Select(c => c.Value.ToString()).ToArray();
        _logger.LogInformation("Headers found: {HeaderCount}", headerRow.Length);
        var headerDictionary = headerRow
            .Select((header, index) => new { header, index })
            .ToDictionary(x => x.header, x => x.index, StringComparer.OrdinalIgnoreCase);
        if (headerRow.Length != requiredHeaders.Length || requiredHeaders.Any(h => !headerDictionary.ContainsKey(h)))
        {
            throw new Exception("Headers do not match");
        }
            
        // All except headers
        var rows = worksheet.RowsUsed().Skip(1);

        var data = rows.Select(r => new MinimalDataDto
        {
            Id = Guid.Parse(r.Cell(1).Value.ToString()),
            OrganisationName = r.Cell(7).Value.ToString(),
            La = r.Cell(8).Value.ToString(),
            Name = r.Cell(2).Value.ToString(),
            Description = r.Cell(3).Value.ToString(),
            Phone = r.Cell(4).Value.ToString(),
            Email = r.Cell(5).Value.ToString(),
            Url = r.Cell(6).Value.ToString(),
        }).ToArray();

        return data;
    }

    public OrgSeedDataDto[] GetSeedOrganisationsFromCsv(string fileName)
    {
        var fileExtension = Path.GetExtension(fileName);
        if (fileExtension != ".csv")
        {
            throw new Exception("File is not a CSV");
        }
        
        var fileData = File.ReadAllLines(fileName);
        
        var headerRow = fileData[0].Split(',');
        _logger.LogInformation("Headers found: {HeaderCount}", headerRow.Length);
        
        var headerDictionary = headerRow
            .Select((header, index) => new { header, index })
            .ToDictionary(x => x.header, x => x.index, StringComparer.OrdinalIgnoreCase);
        
        var requiredHeaders = new[] { "Name", "Address1", "City", "PostalCode", "Country", "StateProvince" };
        
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
            return new OrgSeedDataDto
            {
                Name = columns[headerDictionary["Name"]].Trim(),
                Address1 = columns[headerDictionary["Address1"]].Trim(),
                City = columns[headerDictionary["City"]].Trim(),
                PostalCode = columns[headerDictionary["PostalCode"]].Trim(),
                Country = columns[headerDictionary["Country"]].Trim(),
                StateProvince = columns[headerDictionary["StateProvince"]].Trim(),
            };
        }).ToArray();
        
        return data;
        
        
    }
}