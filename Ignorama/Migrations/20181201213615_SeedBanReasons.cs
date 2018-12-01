using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignorama.Migrations
{
    public partial class SeedBanReasons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "BanReasons",
                new string[] { "Text", "BaseBanHours", "RuleDescription" },
                new object[,] {
                    { "Illegal content", 168, "Do not post anything that is illegal in the United States." },
                    {
                        "Posted thread on wrong board",
                        2,
                        "When creating a new thread, select a board that makes sense.  If there is no board that makes sense for your thread, select Off-Topic."
                    },
                    {
                        "Off-topic reply",
                        2,
                        "When replying to a thread, post should be on-topic for that thread."
                    },
                    { "Spam", 24, "Do not spam." },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                "BanReasons",
                "Text",
                new object[] {
                    "Illegal content",
                    "Posted thread on wrong board",
                    "Off-topic reply",
                    "Spam",
                });
        }
    }
}