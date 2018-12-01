using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class AddBanReasonToContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bans_BanReason_ReasonID",
                table: "Bans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BanReason",
                table: "BanReason");

            migrationBuilder.RenameTable(
                name: "BanReason",
                newName: "BanReasons");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BanReasons",
                table: "BanReasons",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bans_BanReasons_ReasonID",
                table: "Bans",
                column: "ReasonID",
                principalTable: "BanReasons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bans_BanReasons_ReasonID",
                table: "Bans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BanReasons",
                table: "BanReasons");

            migrationBuilder.RenameTable(
                name: "BanReasons",
                newName: "BanReason");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BanReason",
                table: "BanReason",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bans_BanReason_ReasonID",
                table: "Bans",
                column: "ReasonID",
                principalTable: "BanReason",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
