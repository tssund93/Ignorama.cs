using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class AnnouncementsIsAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Tags SET WriteRoleId = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'ADMIN') " +
                "WHERE Name = 'Announcements'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Tags SET WriteRoleId = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'MODERATOR') " +
                "WHERE Name = 'Announcements'");
        }
    }
}