using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.Report.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSearchFacts_OrganisationDim_OrganisationKey",
                table: "ServiceSearchFacts");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSearchFacts_ServiceDim_ServiceKey",
                table: "ServiceSearchFacts");

            migrationBuilder.DropTable(
                name: "OrganisationDim");

            migrationBuilder.DropTable(
                name: "ServiceDim");

            migrationBuilder.DropIndex(
                name: "IX_ServiceSearchFacts_OrganisationKey",
                table: "ServiceSearchFacts");

            migrationBuilder.DropColumn(
                name: "HttpResponseCode",
                table: "ServiceSearchFacts");

            migrationBuilder.DropColumn(
                name: "LocationKey",
                table: "ServiceSearchFacts");

            migrationBuilder.DropColumn(
                name: "OrganisationKey",
                table: "ServiceSearchFacts");

            migrationBuilder.DropColumn(
                name: "SearchId",
                table: "ServiceSearchFacts");

            migrationBuilder.DropColumn(
                name: "SearchPostcode",
                table: "ServiceSearchFacts");

            migrationBuilder.DropColumn(
                name: "SearchRadiusMiles",
                table: "ServiceSearchFacts");

            migrationBuilder.DropColumn(
                name: "ServiceInfoKey",
                table: "ServiceSearchFacts");

            migrationBuilder.DropColumn(
                name: "TaxonomyKey",
                table: "ServiceSearchFacts");

            migrationBuilder.DropColumn(
                name: "UserKey",
                table: "ServiceSearchFacts");

            migrationBuilder.RenameColumn(
                name: "ServiceKey",
                table: "ServiceSearchFacts",
                newName: "ServiceSearchesKey");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceSearchFacts_ServiceKey",
                table: "ServiceSearchFacts",
                newName: "IX_ServiceSearchFacts_ServiceSearchesKey");

            migrationBuilder.RenameColumn(
                name: "WeekOfYear",
                table: "DateDim",
                newName: "WeekNumberOfYear");

            migrationBuilder.RenameColumn(
                name: "MonthOfYear",
                table: "DateDim",
                newName: "MonthNumberOfYear");

            migrationBuilder.RenameColumn(
                name: "DayOfYear",
                table: "DateDim",
                newName: "DayNumberOfYear");

            migrationBuilder.RenameColumn(
                name: "DayOfWeek",
                table: "DateDim",
                newName: "DayNumberOfWeek");

            migrationBuilder.RenameColumn(
                name: "DayOfMonth",
                table: "DateDim",
                newName: "DayNumberOfMonth");

            migrationBuilder.RenameColumn(
                name: "CalendarYear",
                table: "DateDim",
                newName: "CalendarYearNumber");

            migrationBuilder.RenameColumn(
                name: "CalendarQuarter",
                table: "DateDim",
                newName: "CalendarQuarterNumberOfYear");

            migrationBuilder.AlterColumn<int>(
                name: "TimeKey",
                table: "ServiceSearchFacts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.DropPrimaryKey("PK_ServiceSearchFacts", "ServiceSearchFacts");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ServiceSearchFacts",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey("PK_ServiceSearchFacts", "ServiceSearchFacts", "Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ServiceSearchFacts",
                type: "datetime2(7)",
                precision: 7,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "ServiceSearchFacts",
                type: "datetime2(7)",
                precision: 7,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ServiceSearchesDim",
                columns: table => new
                {
                    ServiceSearchesKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceSearchId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    ServiceTypeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EventId = table.Column<long>(type: "bigint", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    RoleTypeId = table.Column<byte>(type: "tinyint", nullable: true),
                    RoleTypeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OrganisationId = table.Column<long>(type: "bigint", nullable: true),
                    OrganisationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OrganisationTypeId = table.Column<byte>(type: "tinyint", nullable: true),
                    OrganisationTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SearchRadiusMiles = table.Column<byte>(type: "tinyint", nullable: false),
                    HttpRequestTimestamp = table.Column<DateTime>(type: "datetime2(7)", precision: 7, nullable: false),
                    HttpRequestCorrelationId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HttpResponseCode = table.Column<short>(type: "smallint", nullable: true),
                    HttpResponseTimestamp = table.Column<DateTime>(type: "datetime2(7)", precision: 7, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2(7)", precision: 7, nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2(7)", precision: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceSearchesDim", x => x.ServiceSearchesKey)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "TimeDim",
                columns: table => new
                {
                    TimeKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeString = table.Column<string>(type: "varchar(8)", nullable: false),
                    HourNumberOfDay = table.Column<byte>(type: "tinyint", nullable: false),
                    MinuteNumberOfHour = table.Column<byte>(type: "tinyint", nullable: false),
                    SecondNumberOfMinute = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeDim", x => x.TimeKey)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSearchFacts_TimeKey",
                table: "ServiceSearchFacts",
                column: "TimeKey");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSearchFacts_ServiceSearchesDim_ServiceSearchesKey",
                table: "ServiceSearchFacts",
                column: "ServiceSearchesKey",
                principalTable: "ServiceSearchesDim",
                principalColumn: "ServiceSearchesKey",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSearchFacts_TimeDim_TimeKey",
                table: "ServiceSearchFacts",
                column: "TimeKey",
                principalTable: "TimeDim",
                principalColumn: "TimeKey",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSearchFacts_ServiceSearchesDim_ServiceSearchesKey",
                table: "ServiceSearchFacts");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSearchFacts_TimeDim_TimeKey",
                table: "ServiceSearchFacts");

            migrationBuilder.DropTable(
                name: "ServiceSearchesDim");

            migrationBuilder.DropTable(
                name: "TimeDim");

            migrationBuilder.DropIndex(
                name: "IX_ServiceSearchFacts_TimeKey",
                table: "ServiceSearchFacts");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ServiceSearchFacts");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "ServiceSearchFacts");

            migrationBuilder.RenameColumn(
                name: "ServiceSearchesKey",
                table: "ServiceSearchFacts",
                newName: "ServiceKey");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceSearchFacts_ServiceSearchesKey",
                table: "ServiceSearchFacts",
                newName: "IX_ServiceSearchFacts_ServiceKey");

            migrationBuilder.RenameColumn(
                name: "WeekNumberOfYear",
                table: "DateDim",
                newName: "WeekOfYear");

            migrationBuilder.RenameColumn(
                name: "MonthNumberOfYear",
                table: "DateDim",
                newName: "MonthOfYear");

            migrationBuilder.RenameColumn(
                name: "DayNumberOfYear",
                table: "DateDim",
                newName: "DayOfYear");

            migrationBuilder.RenameColumn(
                name: "DayNumberOfWeek",
                table: "DateDim",
                newName: "DayOfWeek");

            migrationBuilder.RenameColumn(
                name: "DayNumberOfMonth",
                table: "DateDim",
                newName: "DayOfMonth");

            migrationBuilder.RenameColumn(
                name: "CalendarYearNumber",
                table: "DateDim",
                newName: "CalendarYear");

            migrationBuilder.RenameColumn(
                name: "CalendarQuarterNumberOfYear",
                table: "DateDim",
                newName: "CalendarQuarter");

            migrationBuilder.AlterColumn<int>(
                name: "TimeKey",
                table: "ServiceSearchFacts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ServiceSearchFacts",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "HttpResponseCode",
                table: "ServiceSearchFacts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocationKey",
                table: "ServiceSearchFacts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationKey",
                table: "ServiceSearchFacts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SearchId",
                table: "ServiceSearchFacts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "SearchPostcode",
                table: "ServiceSearchFacts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "SearchRadiusMiles",
                table: "ServiceSearchFacts",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ServiceInfoKey",
                table: "ServiceSearchFacts",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaxonomyKey",
                table: "ServiceSearchFacts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserKey",
                table: "ServiceSearchFacts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrganisationDim",
                columns: table => new
                {
                    OrganisationKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminAreaCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OrganisationTypeId = table.Column<short>(type: "smallint", nullable: false),
                    OrganisationTypeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationDim", x => x.OrganisationKey)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "ServiceDim",
                columns: table => new
                {
                    ServiceKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceProviderId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceProviderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ServiceStatusTypeId = table.Column<byte>(type: "tinyint", nullable: true),
                    ServiceStatusTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ServiceTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    ServiceTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDim", x => x.ServiceKey)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSearchFacts_OrganisationKey",
                table: "ServiceSearchFacts",
                column: "OrganisationKey");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSearchFacts_OrganisationDim_OrganisationKey",
                table: "ServiceSearchFacts",
                column: "OrganisationKey",
                principalTable: "OrganisationDim",
                principalColumn: "OrganisationKey",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSearchFacts_ServiceDim_ServiceKey",
                table: "ServiceSearchFacts",
                column: "ServiceKey",
                principalTable: "ServiceDim",
                principalColumn: "ServiceKey",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
