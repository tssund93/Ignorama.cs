using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class SeedPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "PermissionLevels",
                new string[] { "Name", "Level" },
                new object[,] {
                    { "Administrator", 100 },
                    { "Moderator", 50 },
                    { "User", 20 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                "PermissionLevels",
                "Name",
                new object[] { "Administrator", "Moderator", "User" });
        }
    }
}
