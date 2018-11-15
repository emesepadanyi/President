using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace President.DAL.Migrations
{
    public partial class RemoveGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerGameStatistics");

            migrationBuilder.DropTable(
                name: "GameStatistics");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropColumn(
                name: "TimeSpentPlaying",
                table: "PlayerStatistics");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeSpentPlaying",
                table: "PlayerStatistics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Available = table.Column<bool>(nullable: false),
                    Finished = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GameStatistics",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AvgCardsInHandsWhenWinning = table.Column<double>(nullable: false),
                    DifferenceBWNBestAndWorstPlayer = table.Column<int>(nullable: false),
                    GameID = table.Column<int>(nullable: false),
                    Rounds = table.Column<int>(nullable: false),
                    TimeSpentPlayingSec = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStatistics", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GameStatistics_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_GameStatistics_GameID",
                table: "GameStatistics",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameStatistics_GameStatisticsId",
                table: "PlayerGameStatistics",
                column: "GameStatisticsId");
        }
    }
}
