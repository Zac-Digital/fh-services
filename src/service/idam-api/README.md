# fh-Idam-Api

## Requirements

* DotNet Core 7.0 and any supported IDE for DEV running.

## About

This is a classing Controller Api application for managing user's Additional Claim's. 

## Local running

In the appsetting.json file make sure the database connection string is pointing to your local sql express instance

The startup project is: FamilyHub.Idam.Api

## Migrations Commands

First of all add this package if it does not already exist.
dotnet add package Microsoft.EntityFrameworkCore.Design

To Add Migration

<br />
 dotnet ef migrations add CreateIntialSchema --project ..\FamilyHubs.Idam.data
<br />

To Apply Latest Schema Manually

<br />
 dotnet ef database update --project ..\FamilyHubs.Idam.data
<br />

