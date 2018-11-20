using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class DropPermissionLevelColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PermissionLevels_PermissionLevelID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PermissionLevelID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermissionLevelID",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PermissionLevelID",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PermissionLevelID",
                table: "AspNetUsers",
                column: "PermissionLevelID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PermissionLevels_PermissionLevelID",
                table: "AspNetUsers",
                column: "PermissionLevelID",
                principalTable: "PermissionLevels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
