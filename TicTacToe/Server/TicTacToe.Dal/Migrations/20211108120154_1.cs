using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicTacToe.Dal.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CrossId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ZeroId = table.Column<Guid>(type: "TEXT", nullable: true),
                    NextTurn = table.Column<byte>(type: "INTEGER", nullable: false),
                    Winner = table.Column<byte>(type: "INTEGER", nullable: false),
                    Cells = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}
