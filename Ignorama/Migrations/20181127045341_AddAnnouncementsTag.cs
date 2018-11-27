using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class AddAnnouncementsTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Tags_ReadRoleId", table: "Tags");
            migrationBuilder.DropIndex(name: "IX_Tags_WriteRoleId", table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_IdentityRole_ReadRoleId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_IdentityRole_WriteRoleId",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "IdentityRole");

            migrationBuilder.AlterColumn<long>(
                name: "WriteRoleId",
                table: "Tags",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ReadRoleId",
                table: "Tags",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetRoles_ReadRoleId",
                table: "Tags",
                column: "ReadRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetRoles_WriteRoleId",
                table: "Tags",
                column: "WriteRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("INSERT INTO Tags (Name, ReadRoleId, WriteRoleId, AlwaysVisible, Deleted) " +
                "VALUES ('Announcements', null, (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'MODERATOR'), 1, 0)");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ReadRoleId",
                table: "Tags",
                column: "ReadRoleId");
            migrationBuilder.CreateIndex(
                name: "IX_Tags_WriteRoleId",
                table: "Tags",
                column: "WriteRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Tags_ReadRoleId", table: "Tags");
            migrationBuilder.DropIndex(name: "IX_Tags_WriteRoleId", table: "Tags");

            migrationBuilder.DeleteData(
                   "Tags",
                   "Name",
                   "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetRoles_ReadRoleId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetRoles_WriteRoleId",
                table: "Tags");

            migrationBuilder.AlterColumn<string>(
                name: "WriteRoleId",
                table: "Tags",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReadRoleId",
                table: "Tags",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_IdentityRole_ReadRoleId",
                table: "Tags",
                column: "ReadRoleId",
                principalTable: "IdentityRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_IdentityRole_WriteRoleId",
                table: "Tags",
                column: "WriteRoleId",
                principalTable: "IdentityRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ReadRoleId",
                table: "Tags",
                column: "ReadRoleId");
            migrationBuilder.CreateIndex(
                name: "IX_Tags_WriteRoleId",
                table: "Tags",
                column: "WriteRoleId");
        }
    }
}