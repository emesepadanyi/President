using Microsoft.EntityFrameworkCore.Migrations;

namespace President.DAL.Migrations
{
    public partial class Typo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_AspNetUsers_RecieverId",
                table: "Relationships");

            migrationBuilder.RenameColumn(
                name: "RecieverId",
                table: "Relationships",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Relationships_RecieverId",
                table: "Relationships",
                newName: "IX_Relationships_ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_AspNetUsers_ReceiverId",
                table: "Relationships",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_AspNetUsers_ReceiverId",
                table: "Relationships");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Relationships",
                newName: "RecieverId");

            migrationBuilder.RenameIndex(
                name: "IX_Relationships_ReceiverId",
                table: "Relationships",
                newName: "IX_Relationships_RecieverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_AspNetUsers_RecieverId",
                table: "Relationships",
                column: "RecieverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
