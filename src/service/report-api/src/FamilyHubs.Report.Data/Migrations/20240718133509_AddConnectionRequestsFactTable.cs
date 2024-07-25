using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.Report.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectionRequestsFactTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConnectionRequestsFacts",
                schema: "dim",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateKey = table.Column<int>(type: "int", nullable: false),
                    TimeKey = table.Column<int>(type: "int", nullable: false),
                    OrganisationKey = table.Column<long>(type: "bigint", nullable: false),
                    ConnectionRequestServiceKey = table.Column<long>(type: "bigint", nullable: false),
                    ConnectionRequestStatusTypeKey = table.Column<short>(type: "smallint", nullable: false),
                    ConnectionRequestId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2(7)", precision: 7, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2(7)", precision: 7, nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionRequestsFacts", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_ConnectionRequestsFacts_DateDim_DateKey",
                        column: x => x.DateKey,
                        principalSchema: "dim",
                        principalTable: "DateDim",
                        principalColumn: "DateKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectionRequestsFacts_OrganisationDim_OrganisationKey",
                        column: x => x.OrganisationKey,
                        principalSchema: "idam",
                        principalTable: "OrganisationDim",
                        principalColumn: "OrganisationKey");
                    table.ForeignKey(
                        name: "FK_ConnectionRequestsFacts_TimeDim_TimeKey",
                        column: x => x.TimeKey,
                        principalSchema: "dim",
                        principalTable: "TimeDim",
                        principalColumn: "TimeKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequestsFacts_DateKey",
                schema: "dim",
                table: "ConnectionRequestsFacts",
                column: "DateKey");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequestsFacts_OrganisationKey",
                schema: "dim",
                table: "ConnectionRequestsFacts",
                column: "OrganisationKey");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequestsFacts_TimeKey",
                schema: "dim",
                table: "ConnectionRequestsFacts",
                column: "TimeKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionRequestsFacts",
                schema: "dim");
        }
    }
}
