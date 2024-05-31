using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.Report.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DateDim",
                columns: table => new
                {
                    DateKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateString = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DayOfWeek = table.Column<byte>(type: "tinyint", nullable: false),
                    DayOfWeekName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DayOfMonth = table.Column<byte>(type: "tinyint", nullable: false),
                    DayOfYear = table.Column<short>(type: "smallint", nullable: false),
                    WeekOfYear = table.Column<byte>(type: "tinyint", nullable: false),
                    MonthName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MonthOfYear = table.Column<byte>(type: "tinyint", nullable: false),
                    CalendarQuarter = table.Column<byte>(type: "tinyint", nullable: false),
                    CalendarYear = table.Column<short>(type: "smallint", nullable: false),
                    IsWeekend = table.Column<bool>(type: "bit", nullable: false),
                    IsLeapYear = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateDim", x => x.DateKey)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "OrganisationDim",
                columns: table => new
                {
                    OrganisationKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    OrganisationTypeId = table.Column<short>(type: "smallint", nullable: false),
                    OrganisationTypeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AdminAreaCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationDim", x => x.OrganisationKey)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "ServiceSearchFacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceInfoKey = table.Column<short>(type: "smallint", nullable: true),
                    OrganisationKey = table.Column<int>(type: "int", nullable: false),
                    UserKey = table.Column<int>(type: "int", nullable: true),
                    TaxonomyKey = table.Column<int>(type: "int", nullable: true),
                    LocationKey = table.Column<int>(type: "int", nullable: true),
                    ServiceKey = table.Column<int>(type: "int", nullable: true),
                    DateKey = table.Column<int>(type: "int", nullable: false),
                    TimeKey = table.Column<int>(type: "int", nullable: true),
                    SearchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SearchPostcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SearchRadiusMiles = table.Column<byte>(type: "tinyint", nullable: true),
                    HttpResponseCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceSearchFacts", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_ServiceSearchFacts_DateDim_DateKey",
                        column: x => x.DateKey,
                        principalTable: "DateDim",
                        principalColumn: "DateKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceSearchFacts_OrganisationDim_OrganisationKey",
                        column: x => x.OrganisationKey,
                        principalTable: "OrganisationDim",
                        principalColumn: "OrganisationKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSearchFacts_DateKey",
                table: "ServiceSearchFacts",
                column: "DateKey");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSearchFacts_OrganisationKey",
                table: "ServiceSearchFacts",
                column: "OrganisationKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceSearchFacts");

            migrationBuilder.DropTable(
                name: "DateDim");

            migrationBuilder.DropTable(
                name: "OrganisationDim");
        }
    }
}
