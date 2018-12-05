using Microsoft.EntityFrameworkCore.Migrations;

namespace President.DAL.Migrations
{
    public partial class ChageUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlayerStatistics_UserId",
                table: "PlayerStatistics");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStatistics_UserId",
                table: "PlayerStatistics",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlayerStatistics_UserId",
                table: "PlayerStatistics");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStatistics_UserId",
                table: "PlayerStatistics",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }
    }
}
