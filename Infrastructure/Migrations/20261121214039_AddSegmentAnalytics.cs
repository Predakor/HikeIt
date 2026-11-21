using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSegmentAnalytics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The Pace/Terrain settings were renamed and their filters changed shape; the seeder recreates settings from code defaults.
            migrationBuilder.Sql("DELETE FROM \"AppSettings\";");

            migrationBuilder.AddColumn<double>(
                name: "RouteAnalytics_LongestClimbDistanceM",
                table: "TripAnalytics",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RouteAnalytics_LongestClimbPercent",
                table: "TripAnalytics",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RouteAnalytics_LongestDescentDistanceM",
                table: "TripAnalytics",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RouteAnalytics_LongestDescentPercent",
                table: "TripAnalytics",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RouteAnalytics_SteepestClimbPercent",
                table: "TripAnalytics",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RouteAnalytics_SteepestDescentPercent",
                table: "TripAnalytics",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TimeAnalytics_FastestSpeedKph",
                table: "TripAnalytics",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TimeAnalytics_SlowestSpeedKph",
                table: "TripAnalytics",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"AppSettings\";");

            migrationBuilder.DropColumn(
                name: "RouteAnalytics_LongestClimbDistanceM",
                table: "TripAnalytics");

            migrationBuilder.DropColumn(
                name: "RouteAnalytics_LongestClimbPercent",
                table: "TripAnalytics");

            migrationBuilder.DropColumn(
                name: "RouteAnalytics_LongestDescentDistanceM",
                table: "TripAnalytics");

            migrationBuilder.DropColumn(
                name: "RouteAnalytics_LongestDescentPercent",
                table: "TripAnalytics");

            migrationBuilder.DropColumn(
                name: "RouteAnalytics_SteepestClimbPercent",
                table: "TripAnalytics");

            migrationBuilder.DropColumn(
                name: "RouteAnalytics_SteepestDescentPercent",
                table: "TripAnalytics");

            migrationBuilder.DropColumn(
                name: "TimeAnalytics_FastestSpeedKph",
                table: "TripAnalytics");

            migrationBuilder.DropColumn(
                name: "TimeAnalytics_SlowestSpeedKph",
                table: "TripAnalytics");
        }
    }
}
