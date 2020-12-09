using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicTacToe.Dal.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Cells",
                table: "Game",
                type: "BLOB",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NextTurn",
                table: "Game",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Winner",
                table: "Game",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cells",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "NextTurn",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "Winner",
                table: "Game");
        }
    }
}
