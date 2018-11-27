using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class RequireTagRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetRoles_ReadRoleId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetRoles_WriteRoleId",
                table: "Tags");

            migrationBuilder.AlterColumn<long>(
                name: "WriteRoleId",
                table: "Tags",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ReadRoleId",
                table: "Tags",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetRoles_ReadRoleId",
                table: "Tags",
                column: "ReadRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetRoles_WriteRoleId",
                table: "Tags",
                column: "WriteRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetRoles_ReadRoleId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetRoles_WriteRoleId",
                table: "Tags");

            migrationBuilder.AlterColumn<long>(
                name: "WriteRoleId",
                table: "Tags",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "ReadRoleId",
                table: "Tags",
                nullable: true,
                oldClrType: typeof(long));

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
        }
    }
}