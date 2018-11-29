using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class StoreModeratorInBans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ModeratorId",
                table: "Bans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Bans_ModeratorId",
                table: "Bans",
                column: "ModeratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bans_AspNetUsers_ModeratorId",
                table: "Bans",
                column: "ModeratorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bans_AspNetUsers_ModeratorId",
                table: "Bans");

            migrationBuilder.DropIndex(
                name: "IX_Bans_ModeratorId",
                table: "Bans");

            migrationBuilder.DropColumn(
                name: "ModeratorId",
                table: "Bans");
        }
    }
}
