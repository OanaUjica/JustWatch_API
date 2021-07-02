using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab1_.NET.Migrations
{
    public partial class Update_User_Watchlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieWatchlist_Watchlists_ReservationsId",
                table: "MovieWatchlist");

            migrationBuilder.RenameColumn(
                name: "ReservationsId",
                table: "MovieWatchlist",
                newName: "WatchlistsId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieWatchlist_ReservationsId",
                table: "MovieWatchlist",
                newName: "IX_MovieWatchlist_WatchlistsId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieWatchlist_Watchlists_WatchlistsId",
                table: "MovieWatchlist",
                column: "WatchlistsId",
                principalTable: "Watchlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieWatchlist_Watchlists_WatchlistsId",
                table: "MovieWatchlist");

            migrationBuilder.RenameColumn(
                name: "WatchlistsId",
                table: "MovieWatchlist",
                newName: "ReservationsId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieWatchlist_WatchlistsId",
                table: "MovieWatchlist",
                newName: "IX_MovieWatchlist_ReservationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieWatchlist_Watchlists_ReservationsId",
                table: "MovieWatchlist",
                column: "ReservationsId",
                principalTable: "Watchlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
