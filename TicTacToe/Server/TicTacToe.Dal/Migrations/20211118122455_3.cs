using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Dal.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserEntity_Name",
                table: "UserEntity");

            migrationBuilder.CreateIndex(
                name: "IX_UserEntity_Name",
                table: "UserEntity",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserEntity_Name",
                table: "UserEntity");

            migrationBuilder.CreateIndex(
                name: "IX_UserEntity_Name",
                table: "UserEntity",
                column: "Name");
        }
    }
}
