using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BullMonitor.Data.Sql.Migrations
{
    /// <inheritdoc />
    public partial class InitialTickerSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exchange",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exchange", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sector",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sector", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Industry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Industry_Sector_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ticker",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndustryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExchangeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticker_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticker_Exchange_ExchangeId",
                        column: x => x.ExchangeId,
                        principalTable: "Exchange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticker_Industry_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currency_Code",
                table: "Currency",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currency_Id",
                table: "Currency",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exchange_Code",
                table: "Exchange",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exchange_Id",
                table: "Exchange",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Industry_Code",
                table: "Industry",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Industry_Id",
                table: "Industry",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Industry_SectorId",
                table: "Industry",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sector_Id",
                table: "Sector",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticker_Code",
                table: "Ticker",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticker_CurrencyId",
                table: "Ticker",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticker_ExchangeId",
                table: "Ticker",
                column: "ExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticker_Id",
                table: "Ticker",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticker_IndustryId",
                table: "Ticker",
                column: "IndustryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ticker");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "Exchange");

            migrationBuilder.DropTable(
                name: "Industry");

            migrationBuilder.DropTable(
                name: "Sector");
        }
    }
}
