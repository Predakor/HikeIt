using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UnitNameInColNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_TotalDistanceKm",
                table: "TripAnalytics",
                newName: "RouteAnalytics_TotalDistanceMeters");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_TotalDescent",
                table: "TripAnalytics",
                newName: "RouteAnalytics_TotalDescentMeters");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_TotalAscent",
                table: "TripAnalytics",
                newName: "RouteAnalytics_TotalAscentMeters");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_LowestElevation",
                table: "TripAnalytics",
                newName: "RouteAnalytics_LowestElevationMeters");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_HighestElevation",
                table: "TripAnalytics",
                newName: "RouteAnalytics_HighestElevationMeters");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_AverageSlope",
                table: "TripAnalytics",
                newName: "RouteAnalytics_AverageSlopePercent");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_AverageDescentSlope",
                table: "TripAnalytics",
                newName: "RouteAnalytics_AverageDescentSlopePercent");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_AverageAscentSlope",
                table: "TripAnalytics",
                newName: "RouteAnalytics_AverageAscentSlopePercent");

            migrationBuilder.CreateTable(
                name: "UserStats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalTrips = table.Column<long>(type: "bigint", nullable: false),
                    TotalDistanceM = table.Column<long>(type: "bigint", nullable: false),
                    TotalAscentMeters = table.Column<long>(type: "bigint", nullable: false),
                    TotalDescentMeters = table.Column<long>(type: "bigint", nullable: false),
                    TotalPeaks = table.Column<long>(type: "bigint", nullable: false),
                    TotalDuration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    UniquePeaks = table.Column<long>(type: "bigint", nullable: false),
                    RegionsVisited = table.Column<long>(type: "bigint", nullable: false),
                    FirstHikeDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LastHikeDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LongestTripMeters = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStats_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStats");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_TotalDistanceMeters",
                table: "TripAnalytics",
                newName: "RouteAnalytics_TotalDistanceKm");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_TotalDescentMeters",
                table: "TripAnalytics",
                newName: "RouteAnalytics_TotalDescent");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_TotalAscentMeters",
                table: "TripAnalytics",
                newName: "RouteAnalytics_TotalAscent");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_LowestElevationMeters",
                table: "TripAnalytics",
                newName: "RouteAnalytics_LowestElevation");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_HighestElevationMeters",
                table: "TripAnalytics",
                newName: "RouteAnalytics_HighestElevation");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_AverageSlopePercent",
                table: "TripAnalytics",
                newName: "RouteAnalytics_AverageSlope");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_AverageDescentSlopePercent",
                table: "TripAnalytics",
                newName: "RouteAnalytics_AverageDescentSlope");

            migrationBuilder.RenameColumn(
                name: "RouteAnalytics_AverageAscentSlopePercent",
                table: "TripAnalytics",
                newName: "RouteAnalytics_AverageAscentSlope");
        }
    }
}
