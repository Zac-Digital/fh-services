using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.Idam.Data.Migrations
{
    public partial class AddAdminUICache : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
                CREATE TABLE [dbo].[AdminUiCache](
                    [Id] [nvarchar](449) NOT NULL,
                    [Value] [varbinary](max) NOT NULL,
                    [ExpiresAtTime] [datetimeoffset] NOT NULL,
                    [SlidingExpirationInSeconds] [bigint] NULL,
                    [AbsoluteExpiration] [datetimeoffset] NULL,
                    INDEX Ix_AdminUiCache_ExpiresAtTime NONCLUSTERED ([ExpiresAtTime]),
                    CONSTRAINT Pk_AdminUiCache_Id PRIMARY KEY CLUSTERED ([Id] ASC) WITH 
                        (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
                         IGNORE_DUP_KEY = OFF,
                         ALLOW_ROW_LOCKS = ON,
                         ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];");
        }
        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE [dbo].[AdminUiCache]");
        }
    }
}
