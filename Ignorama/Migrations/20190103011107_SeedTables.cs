using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Ignorama.Migrations
{
    public partial class SeedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "AspNetRoles",
                new string[] { "Name", "NormalizedName", "ConcurrencyStamp" },
                new object[,]
                {
                    { "Admin", "ADMIN", Guid.NewGuid().ToString() },
                    { "Moderator", "MODERATOR", Guid.NewGuid().ToString() },
                    { "User", "USER", Guid.NewGuid().ToString() },
                });

            migrationBuilder.Sql("INSERT INTO \"Tags\" (\"Name\", \"Deleted\", \"WriteRoleId\", \"ReadRoleId\", \"AlwaysVisible\") " +
                "VALUES ('Technology', false, (SELECT \"Id\" FROM \"AspNetRoles\" WHERE \"NormalizedName\" = 'USER'), (SELECT \"Id\" FROM \"AspNetRoles\" WHERE \"NormalizedName\" = 'USER'), false)");
            migrationBuilder.Sql("INSERT INTO \"Tags\" (\"Name\", \"Deleted\", \"WriteRoleId\", \"ReadRoleId\", \"AlwaysVisible\") " +
                "VALUES ('Anime & Manga', false, (SELECT \"Id\" FROM \"AspNetRoles\" WHERE \"NormalizedName\" = 'USER'), (SELECT \"Id\" FROM \"AspNetRoles\" WHERE \"NormalizedName\" = 'USER'), false)");
            migrationBuilder.Sql("INSERT INTO \"Tags\" (\"Name\", \"Deleted\", \"WriteRoleId\", \"ReadRoleId\", \"AlwaysVisible\") " +
                "VALUES ('Video Games', false, (SELECT \"Id\" FROM \"AspNetRoles\" WHERE \"NormalizedName\" = 'USER'), (SELECT \"Id\" FROM \"AspNetRoles\" WHERE \"NormalizedName\" = 'USER'), false)");
            migrationBuilder.Sql("INSERT INTO \"Tags\" (\"Name\", \"Deleted\", \"WriteRoleId\", \"ReadRoleId\", \"AlwaysVisible\") " +
                "VALUES ('Music', false, (SELECT \"Id\" FROM \"AspNetRoles\" WHERE \"NormalizedName\" = 'USER'), (SELECT \"Id\" FROM \"AspNetRoles\" WHERE \"NormalizedName\" = 'USER'), false)");
            migrationBuilder.Sql("INSERT INTO \"Tags\" (\"Name\", \"Deleted\", \"WriteRoleId\", \"ReadRoleId\", \"AlwaysVisible\") " +
                "VALUES ('Off-Topic', false, (SELECT \"Id\" FROM \"AspNetRoles\" WHERE \"NormalizedName\" = 'USER'), (SELECT \"Id\" FROM \"AspNetRoles\" WHERE \"NormalizedName\" = 'USER'), false)");
            migrationBuilder.Sql("INSERT INTO \"Tags\" (\"Name\", \"Deleted\", \"WriteRoleId\", \"ReadRoleId\", \"AlwaysVisible\") " +
                "VALUES ('Announcements', false, (SELECT \"Id\" FROM \"AspNetRoles\" WHERE \"NormalizedName\" = 'ADMIN'), (SELECT \"Id\" FROM \"AspNetRoles\" WHERE \"NormalizedName\" = 'USER'), true)");

            migrationBuilder.InsertData(
                "BanReasons",
                new string[] { "Text", "BaseBanHours", "RuleDescription" },
                new object[,] {
                    { "Illegal content", 168, "Do not post anything that is illegal in the United States." },
                    {
                        "Posted thread on wrong board",
                        2,
                        "When creating a new thread, select a board that makes sense.  If there is no board that makes sense for your thread, select Off-Topic.",
                    },
                    {
                        "Off-topic reply",
                        2,
                        "When replying to a thread, post should be on-topic for that thread."
                    },
                    { "Spam", 24, "Do not spam." },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                "Tags",
                "Name",
                new object[] { "Technology", "Anime & Manga", "Video Games", "Music", "Off-Topic", "Announcements" });

            migrationBuilder.DeleteData(
                "AspNetRoles",
                "Name",
                new object[] { "Admin", "Moderator", "User" });

            migrationBuilder.DeleteData(
                "BanReasons",
                "Text",
                new object[] {
                    "Illegal content",
                    "Posted thread on wrong board",
                    "Off-topic reply",
                    "Spam",
                });
        }
    }
}