using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class RemoveUserIPFromBans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bans_AspNetUsers_UserId",
                table: "Bans");

            migrationBuilder.DropIndex(
                name: "IX_Bans_UserId",
                table: "Bans");

            migrationBuilder.DropColumn(
                name: "IP",
                table: "Bans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bans");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IP",
                table: "Bans",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Bans",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bans_UserId",
                table: "Bans",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bans_AspNetUsers_UserId",
                table: "Bans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
