using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.Referral.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF NOT EXISTS (SELECT * FROM Statuses)
                BEGIN
                    INSERT INTO Statuses (Id, Name, SortOrder, SecondrySortOrder) VALUES
                        (1, 'New', 0, 1),
                        (2, 'Opened', 1, 1),
                        (3, 'Accepted', 2, 2),
                        (4, 'Declined', 3, 0);
                END;
            """);
            
            migrationBuilder.Sql("""
                IF NOT EXISTS (SELECT * FROM Roles)
                BEGIN
                    INSERT INTO Roles (Id, Name, Description) VALUES
                        (1, 'DfeAdmin', 'DfE Administrator'),
                        (2, 'LaManager', 'Local Authority Manager'),
                        (3, 'VcsManager', 'VCS Manager'),
                        (4, 'LaProfessional', 'Local Authority Professional'),
                        (5, 'VcsProfessional', 'VCS Professional'),
                        (6, 'LaDualRole', 'Local Authority Dual Role'),
                        (7, 'VcsDualRole', 'VCS Dual Role');
                END;
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Not supported
        }
    }
}
