# File upload tool

## Description

Simple console application to upload files to the service directory DEDS database.
The files are read from the local file system.

The file should be in XLSX format and contain a headers row with the headers found in the `FileReaderService` class. They are not required to be in order but the headers should be present in the file.

This application runs on Create only if new basis. Any existing services will be ignored. It's recommended to clear down your deds tables before running this application, so you can see the new services being created.

## Usage

Depending on when this is run, there is a migration to be run on the database. This is done through the `dotnet ef database update` command from the service/service-directory-api project. This will update a field that needed to be changed.

The 'Manual data load spreadsheet' (not shown here locally) has all the data in xlsx that matches the headers in the `FileReaderService` class. This is the file that should be used to upload the data.

The `SeedOrgsFilePath` in the `appsettings.json` file is the path to the file that contains the seed organisations. This is used to seed the organisations table in the database. The file should be in CSV format.
Match the organisation names found in the 'Manual data load' spreadsheet with the names in the seed organisations file. 
As this is a prototype we just do name matching instead of using the organisation id.

Seed data can be found in confluence document: File upload data discovery

1. Update the `appsettings.json` file with the correct configuration values.

    ```json
    {
      "FilePath": "./data/relativeOrAboslutePathToFile.xlsx",
      "SeedOrgsFilePath": "./data/seed_organisations_relative_or_absolute_path.csv",
      "ConnectionStrings": {
        "DefaultConnection": "database connection string to local or a cloud environment - target service directory database"
      }
    }
    ```

2. Run the application through IDE or command line. There are no program arguments as all the configuration are in the appsettings.json