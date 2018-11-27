using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class SetWriteRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Tags SET WriteRoleId = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'USER') " +
                "WHERE Name <> 'Announcements'");
            migrationBuilder.Sql("UPDATE Tags SET ReadRoleId = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'USER') ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Tags SET WriteRoleId = null " +
                "WHERE Name <> 'Announcements'");
            migrationBuilder.Sql("UPDATE Tags SET ReadRoleId = null");
        }
    }
}