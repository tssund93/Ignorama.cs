using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class AddPermissionLevels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequiredPermissionLevel",
                table: "Tags",
                newName: "RequiredPermissionLevelID");

            migrationBuilder.CreateTable(
                name: "PermissionLevels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionLevels", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_RequiredPermissionLevelID",
                table: "Tags",
                column: "RequiredPermissionLevelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_PermissionLevels_RequiredPermissionLevelID",
                table: "Tags",
                column: "RequiredPermissionLevelID",
                principalTable: "PermissionLevels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_PermissionLevels_RequiredPermissionLevelID",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "PermissionLevels");

            migrationBuilder.DropIndex(
                name: "IX_Tags_RequiredPermissionLevelID",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "RequiredPermissionLevelID",
                table: "Tags",
                newName: "RequiredPermissionLevel");
        }
    }
}
