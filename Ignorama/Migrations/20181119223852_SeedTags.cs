using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class SeedTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Tags (Name, RequiredPermissionLevelID, Deleted) " +
                "VALUES ('Technology', (SELECT ID FROM PermissionLevels WHERE Name = 'User'), 0)");
            migrationBuilder.Sql("INSERT INTO Tags (Name, RequiredPermissionLevelID, Deleted) " +
                "VALUES ('Anime & Manga', (SELECT ID FROM PermissionLevels WHERE Name = 'User'), 0)");
            migrationBuilder.Sql("INSERT INTO Tags (Name, RequiredPermissionLevelID, Deleted) " +
                "VALUES ('Video Games', (SELECT ID FROM PermissionLevels WHERE Name = 'User'), 0)");
            migrationBuilder.Sql("INSERT INTO Tags (Name, RequiredPermissionLevelID, Deleted) " +
                "VALUES ('Music', (SELECT ID FROM PermissionLevels WHERE Name = 'User'), 0)");
            migrationBuilder.Sql("INSERT INTO Tags (Name, RequiredPermissionLevelID, Deleted) " +
                "VALUES ('Off-Topic', (SELECT ID FROM PermissionLevels WHERE Name = 'User'), 0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                "Tags",
                "Name",
                new object[] { "Technology", "Anime & Manga", "Video Games", "Music", "Off-Topic" });
        }
    }
}
