using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class FollowingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FollowedThreads",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ThreadID = table.Column<int>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    LastSeenPostID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowedThreads", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FollowedThreads_Posts_LastSeenPostID",
                        column: x => x.LastSeenPostID,
                        principalTable: "Posts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowedThreads_Threads_ThreadID",
                        column: x => x.ThreadID,
                        principalTable: "Threads",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowedThreads_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowedThreads_LastSeenPostID",
                table: "FollowedThreads",
                column: "LastSeenPostID");

            migrationBuilder.CreateIndex(
                name: "IX_FollowedThreads_ThreadID",
                table: "FollowedThreads",
                column: "ThreadID");

            migrationBuilder.CreateIndex(
                name: "IX_FollowedThreads_UserId",
                table: "FollowedThreads",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowedThreads");
        }
    }
}
