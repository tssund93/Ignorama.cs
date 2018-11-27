using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class UpdateTagsColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_PermissionLevels_RequiredPermissionLevelID",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_RequiredPermissionLevelID",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "RequiredPermissionLevelID",
                table: "Tags");

            migrationBuilder.AddColumn<bool>(
                name: "AlwaysVisible",
                table: "Tags",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReadRoleId",
                table: "Tags",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WriteRoleId",
                table: "Tags",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ReadRoleId",
                table: "Tags",
                column: "ReadRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_WriteRoleId",
                table: "Tags",
                column: "WriteRoleId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_IdentityRole_ReadRoleId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_IdentityRole_WriteRoleId",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "IdentityRole");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ReadRoleId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_WriteRoleId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "AlwaysVisible",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ReadRoleId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "WriteRoleId",
                table: "Tags");

            migrationBuilder.AddColumn<int>(
                name: "RequiredPermissionLevelID",
                table: "Tags",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

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
                onDelete: ReferentialAction.Restrict);
        }
    }
}
