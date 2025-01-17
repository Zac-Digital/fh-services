# File upload tool

## Description

Simple console application to upload files to the service directory DEDS database.
The files are read from the local file system.

The file should be in CSV format and contain a headers row with the headers found in the `FileReaderService` class. They are not required to be in order but the headers should be present in the file.

This application runs on an Upsert basis. Any existing services will be updated with the data from the file.

## Usage

Depending on when this is run, there is a migration to be run on the database. This is done through the `dotnet ef database update` command. This will update a field that needed to be changed.

1. Update the `appsettings.json` file with the correct configuration values.

    ```json
    {
      "FilePath": "relativeOrAboslutePathToFile.csv",
      "ConnectionStrings": {
        "DefaultConnection": "database connection string to local or a cloud environment - target service directory database"
      }
    }
    ```

2. Run the application through IDE or command line. There are no program arguments as all the configuration is in the appsettings.json