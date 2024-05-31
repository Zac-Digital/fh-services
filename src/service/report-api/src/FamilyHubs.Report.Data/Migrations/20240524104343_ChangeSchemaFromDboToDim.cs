using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.Report.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSchemaFromDboToDim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dim");

            migrationBuilder.RenameTable(
                name: "TimeDim",
                newName: "TimeDim",
                newSchema: "dim");

            migrationBuilder.RenameTable(
                name: "ServiceSearchFacts",
                newName: "ServiceSearchFacts",
                newSchema: "dim");

            migrationBuilder.RenameTable(
                name: "ServiceSearchesDim",
                newName: "ServiceSearchesDim",
                newSchema: "dim");

            migrationBuilder.RenameTable(
                name: "DateDim",
                newName: "DateDim",
                newSchema: "dim");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TimeDim",
                schema: "dim",
                newName: "TimeDim");

            migrationBuilder.RenameTable(
                name: "ServiceSearchFacts",
                schema: "dim",
                newName: "ServiceSearchFacts");

            migrationBuilder.RenameTable(
                name: "ServiceSearchesDim",
                schema: "dim",
                newName: "ServiceSearchesDim");

            migrationBuilder.RenameTable(
                name: "DateDim",
                schema: "dim",
                newName: "DateDim");
        }
    }
}
