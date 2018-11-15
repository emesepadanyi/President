using Microsoft.EntityFrameworkCore.Migrations;

namespace President.DAL.Migrations
{
    public partial class Statistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Rounds",
                table: "GameStatistics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PlayerGameStatistics",
                columns: table => new
                {
                    PlayerStatisticsId = table.Column<int>(nullable: false),
                    GameStatisticsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGameStatistics", x => new { x.PlayerStatisticsId, x.GameStatisticsId });
                    table.ForeignKey(
                        name: "FK_PlayerGameStatistics_GameStatistics_GameStatisticsId",
                        column: x => x.GameStatisticsId,
                        principalTable: "GameStatistics",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerGameStatistics_PlayerStatistics_PlayerStatisticsId",
                        column: x => x.PlayerStatisticsId,
                        principalTable: "PlayerStatistics",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameStatistics_GameStatisticsId",
                table: "PlayerGameStatistics",
                column: "GameStatisticsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerGameStatistics");

            migrationBuilder.DropColumn(
                name: "Rounds",
                table: "GameStatistics");

            migrationBuilder.AddColumn<long>(
                name: "FacebookId",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
