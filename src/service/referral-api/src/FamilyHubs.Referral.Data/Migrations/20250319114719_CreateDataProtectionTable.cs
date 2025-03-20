using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.Referral.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDataProtectionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SharedKernelMigrationsHistory')
                                 BEGIN
                                     CREATE TABLE [dbo].[SharedKernelMigrationsHistory] (
                                         [MigrationId] [nvarchar](150) PRIMARY KEY CLUSTERED NOT NULL,
                                         [ProductVersion] [nvarchar](32) NOT NULL
                                     );
                                 END;
                                 """);
            
            migrationBuilder.Sql("""
                                 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DataProtectionKeys')
                                 BEGIN
                                     CREATE TABLE [dbo].[DataProtectionKeys] (
                                         [Id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED NOT NULL,
                                         [FriendlyName] [nvarchar](max) NULL,
                                         [Xml] [nvarchar](max) NULL
                                     );
                                 END;
                                 """);
            
            migrationBuilder.Sql("""
                                 IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SharedKernelMigrationsHistory')
                                 BEGIN
                                    IF (NOT EXISTS (SELECT 1 FROM [dbo].[SharedKernelMigrationsHistory])) 
                                    BEGIN 
                                        INSERT INTO [dbo].[SharedKernelMigrationsHistory] ([MigrationId], [ProductVersion])
                                        VALUES ('20230802105219_AddDataProtectionKeys', '8.0.8');
                                    END;
                                 END;
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Method intentionally left empty.
        }
    }
}
