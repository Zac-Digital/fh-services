using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.Report.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceDim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ServiceKey",
                table: "ServiceSearchFacts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ServiceDim",
                columns: table => new
                {
                    ServiceKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    ServiceTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServiceStatusTypeId = table.Column<byte>(type: "tinyint", nullable: true),
                    ServiceStatusTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ServiceProviderId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceProviderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDim", x => x.ServiceKey)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSearchFacts_ServiceKey",
                table: "ServiceSearchFacts",
                column: "ServiceKey");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSearchFacts_ServiceDim_ServiceKey",
                table: "ServiceSearchFacts",
                column: "ServiceKey",
                principalTable: "ServiceDim",
                principalColumn: "ServiceKey",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSearchFacts_ServiceDim_ServiceKey",
                table: "ServiceSearchFacts");

            migrationBuilder.DropTable(
                name: "ServiceDim");

            migrationBuilder.DropIndex(
                name: "IX_ServiceSearchFacts_ServiceKey",
                table: "ServiceSearchFacts");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceKey",
                table: "ServiceSearchFacts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
