using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExplicitFksToTripANalytics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElevationProfiles_TripAnalytics_Id",
                table: "ElevationProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_PeaksAnalytic_TripAnalytics_Id",
                table: "PeaksAnalytic");

            migrationBuilder.AddColumn<Guid>(
                name: "ElevationProfileId",
                table: "TripAnalytics",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PeaksAnalyticsId",
                table: "TripAnalytics",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripAnalytics_ElevationProfileId",
                table: "TripAnalytics",
                column: "ElevationProfileId",
                unique: true,
                filter: "[ElevationProfileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TripAnalytics_PeaksAnalyticsId",
                table: "TripAnalytics",
                column: "PeaksAnalyticsId",
                unique: true,
                filter: "[PeaksAnalyticsId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_TripAnalytics_ElevationProfiles_ElevationProfileId",
                table: "TripAnalytics",
                column: "ElevationProfileId",
                principalTable: "ElevationProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TripAnalytics_PeaksAnalytic_PeaksAnalyticsId",
                table: "TripAnalytics",
                column: "PeaksAnalyticsId",
                principalTable: "PeaksAnalytic",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripAnalytics_ElevationProfiles_ElevationProfileId",
                table: "TripAnalytics");

            migrationBuilder.DropForeignKey(
                name: "FK_TripAnalytics_PeaksAnalytic_PeaksAnalyticsId",
                table: "TripAnalytics");

            migrationBuilder.DropIndex(
                name: "IX_TripAnalytics_ElevationProfileId",
                table: "TripAnalytics");

            migrationBuilder.DropIndex(
                name: "IX_TripAnalytics_PeaksAnalyticsId",
                table: "TripAnalytics");

            migrationBuilder.DropColumn(
                name: "ElevationProfileId",
                table: "TripAnalytics");

            migrationBuilder.DropColumn(
                name: "PeaksAnalyticsId",
                table: "TripAnalytics");

            migrationBuilder.AddForeignKey(
                name: "FK_ElevationProfiles_TripAnalytics_Id",
                table: "ElevationProfiles",
                column: "Id",
                principalTable: "TripAnalytics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PeaksAnalytic_TripAnalytics_Id",
                table: "PeaksAnalytic",
                column: "Id",
                principalTable: "TripAnalytics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
