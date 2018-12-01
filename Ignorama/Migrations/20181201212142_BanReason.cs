using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class BanReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "Bans",
                newName: "Details");

            migrationBuilder.AddColumn<int>(
                name: "ReasonID",
                table: "Bans",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BanReason",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true),
                    BaseBanHours = table.Column<int>(nullable: false),
                    RuleDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanReason", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bans_ReasonID",
                table: "Bans",
                column: "ReasonID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bans_BanReason_ReasonID",
                table: "Bans",
                column: "ReasonID",
                principalTable: "BanReason",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bans_BanReason_ReasonID",
                table: "Bans");

            migrationBuilder.DropTable(
                name: "BanReason");

            migrationBuilder.DropIndex(
                name: "IX_Bans_ReasonID",
                table: "Bans");

            migrationBuilder.DropColumn(
                name: "ReasonID",
                table: "Bans");

            migrationBuilder.RenameColumn(
                name: "Details",
                table: "Bans",
                newName: "Reason");
        }
    }
}
