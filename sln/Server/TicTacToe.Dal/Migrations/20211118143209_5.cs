using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Dal.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ZeroId",
                table: "Game",
                newName: "ZeroPlayerId");

            migrationBuilder.RenameColumn(
                name: "CrossId",
                table: "Game",
                newName: "CrossPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_CrossPlayerId",
                table: "Game",
                column: "CrossPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_ZeroPlayerId",
                table: "Game",
                column: "ZeroPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_UserEntity_CrossPlayerId",
                table: "Game",
                column: "CrossPlayerId",
                principalTable: "UserEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_UserEntity_ZeroPlayerId",
                table: "Game",
                column: "ZeroPlayerId",
                principalTable: "UserEntity",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_UserEntity_CrossPlayerId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_UserEntity_ZeroPlayerId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_CrossPlayerId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_ZeroPlayerId",
                table: "Game");

            migrationBuilder.RenameColumn(
                name: "ZeroPlayerId",
                table: "Game",
                newName: "ZeroId");

            migrationBuilder.RenameColumn(
                name: "CrossPlayerId",
                table: "Game",
                newName: "CrossId");
        }
    }
}
