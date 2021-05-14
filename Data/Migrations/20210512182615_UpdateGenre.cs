using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab1_.NET.Data.Migrations
{
    public partial class UpdateGenre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "Movies",
                type: "nvarchar(20)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Genre",
                table: "Movies",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");
        }
    }
}
