using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class UserPermissionLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_PermissionLevels_RequiredPermissionLevelID",
                table: "Tags");

            migrationBuilder.AddColumn<int>(
                name: "PermissionLevelID",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RequiredPermissionLevelID",
                table: "Tags",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Users_PermissionLevelID",
                table: "Users",
                column: "PermissionLevelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_PermissionLevels_RequiredPermissionLevelID",
                table: "Tags",
                column: "RequiredPermissionLevelID",
                principalTable: "PermissionLevels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_PermissionLevels_PermissionLevelID",
                table: "Users",
                column: "PermissionLevelID",
                principalTable: "PermissionLevels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_PermissionLevels_RequiredPermissionLevelID",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_PermissionLevels_PermissionLevelID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PermissionLevelID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PermissionLevelID",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "RequiredPermissionLevelID",
                table: "Tags",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_PermissionLevels_RequiredPermissionLevelID",
                table: "Tags",
                column: "RequiredPermissionLevelID",
                principalTable: "PermissionLevels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
