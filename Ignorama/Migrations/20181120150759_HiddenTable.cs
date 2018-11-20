using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class HiddenTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HiddenThreads",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    ThreadID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HiddenThreads", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HiddenThreads_Threads_ThreadID",
                        column: x => x.ThreadID,
                        principalTable: "Threads",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HiddenThreads_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HiddenThreads_ThreadID",
                table: "HiddenThreads",
                column: "ThreadID");

            migrationBuilder.CreateIndex(
                name: "IX_HiddenThreads_UserId",
                table: "HiddenThreads",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HiddenThreads");
        }
    }
}
