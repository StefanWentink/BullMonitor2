using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BullMonitor.Data.Sql.Migrations
{
    /// <inheritdoc />
    public partial class TickerKnownBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "KnownByTipRanks",
                table: "Ticker",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "KnownByZacks",
                table: "Ticker",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KnownByTipRanks",
                table: "Ticker");

            migrationBuilder.DropColumn(
                name: "KnownByZacks",
                table: "Ticker");
        }
    }
}
