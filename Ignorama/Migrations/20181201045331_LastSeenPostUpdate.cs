using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class LastSeenPostUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowedThreads_Posts_LastSeenPostID",
                table: "FollowedThreads");

            migrationBuilder.DropIndex(
                name: "IX_FollowedThreads_LastSeenPostID",
                table: "FollowedThreads");

            migrationBuilder.AlterColumn<long>(
                name: "LastSeenPostID",
                table: "FollowedThreads",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LastSeenPostID",
                table: "FollowedThreads",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.CreateIndex(
                name: "IX_FollowedThreads_LastSeenPostID",
                table: "FollowedThreads",
                column: "LastSeenPostID");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowedThreads_Posts_LastSeenPostID",
                table: "FollowedThreads",
                column: "LastSeenPostID",
                principalTable: "Posts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
