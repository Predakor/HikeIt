using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PipelineSettingsAndSegmentAnalytics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Setting types change from numbers to names and the filter lists changed shape; the seeder recreates settings from code defaults.
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

            migrationBuilder.AlterColumn<string>(
                name: "SettingType",
                table: "AppSettings",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "JsonValue",
                table: "AppSettings",
                type: "json",
                nullable: false,
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb");

            migrationBuilder.CreateIndex(
                name: "IX_Peaks_Location",
                table: "Peaks",
                column: "Location")
                .Annotation("Npgsql:IndexMethod", "gist");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Names cannot be cast back to ints; the seeder re-creates the rows.
            migrationBuilder.Sql("DELETE FROM \"AppSettings\";");

            migrationBuilder.DropIndex(
                name: "IX_Peaks_Location",
                table: "Peaks");

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

            // The table is empty after the delete above, so the cast never sees a value; EF's own AlterColumn lacks the USING clause Postgres demands.
            migrationBuilder.Sql("ALTER TABLE \"AppSettings\" ALTER COLUMN \"SettingType\" TYPE integer USING 0;");

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "JsonValue",
                table: "AppSettings",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(JsonDocument),
                oldType: "json");
        }
    }
}
