using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Ignorama.Migrations
{
    public partial class DeletedTimeIsDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "Posts");
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTime",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "Posts");
            migrationBuilder.AddColumn<bool>(
                name: "DeletedTime",
                table: "Posts",
                nullable: false);
        }
    }
}