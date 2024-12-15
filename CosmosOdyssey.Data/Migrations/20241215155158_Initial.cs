using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmosOdyssey.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ValidUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceListLeg",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceListId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceListLeg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceListLeg_PriceLists_PriceListId",
                        column: x => x.PriceListId,
                        principalTable: "PriceLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegProvider",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceListId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    FlightStart = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FlightEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PriceListLegId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegProvider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegProvider_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LegProvider_PriceListLeg_PriceListLegId",
                        column: x => x.PriceListLegId,
                        principalTable: "PriceListLeg",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LegRoute",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceListId = table.Column<Guid>(type: "uuid", nullable: false),
                    Distance = table.Column<double>(type: "double precision", nullable: false),
                    PriceListLegId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegRoute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegRoute_PriceListLeg_PriceListLegId",
                        column: x => x.PriceListLegId,
                        principalTable: "PriceListLeg",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegProvider_CompanyId",
                table: "LegProvider",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_LegProvider_PriceListLegId",
                table: "LegProvider",
                column: "PriceListLegId");

            migrationBuilder.CreateIndex(
                name: "IX_LegRoute_PriceListLegId",
                table: "LegRoute",
                column: "PriceListLegId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListLeg_PriceListId",
                table: "PriceListLeg",
                column: "PriceListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegProvider");

            migrationBuilder.DropTable(
                name: "LegRoute");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "PriceListLeg");

            migrationBuilder.DropTable(
                name: "PriceLists");
        }
    }
}
