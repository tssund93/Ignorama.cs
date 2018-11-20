using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class RequireHiddenThread : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HiddenThreads_Threads_ThreadID",
                table: "HiddenThreads");

            migrationBuilder.AlterColumn<int>(
                name: "ThreadID",
                table: "HiddenThreads",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HiddenThreads_Threads_ThreadID",
                table: "HiddenThreads",
                column: "ThreadID",
                principalTable: "Threads",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HiddenThreads_Threads_ThreadID",
                table: "HiddenThreads");

            migrationBuilder.AlterColumn<int>(
                name: "ThreadID",
                table: "HiddenThreads",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_HiddenThreads_Threads_ThreadID",
                table: "HiddenThreads",
                column: "ThreadID",
                principalTable: "Threads",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
