using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRouteAnalyticsSteepestSegment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The setting types were renamed (TimeAnalytics -> Pace, RouteAnalytics -> Terrain) and the old filters are gone; the startup seeder re-creates every setting from its code defaults.
            migrationBuilder.Sql("DELETE FROM \"AppSettings\";");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // The rows hold the new type names; the seeder re-creates them.
            migrationBuilder.Sql("DELETE FROM \"AppSettings\";");

            migrationBuilder.DropColumn(
                name: "RouteAnalytics_SteepestClimbPercent",
                table: "TripAnalytics");

            migrationBuilder.DropColumn(
                name: "RouteAnalytics_SteepestDescentPercent",
                table: "TripAnalytics");
        }
    }
}
