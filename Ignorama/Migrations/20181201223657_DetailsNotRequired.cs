using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class DetailsNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "Bans",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "Bans",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
