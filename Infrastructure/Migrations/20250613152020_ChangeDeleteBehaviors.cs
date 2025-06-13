using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeleteBehaviors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripAnalytics_PeaksAnalytic_PeaksAnalyticsId",
                table: "TripAnalytics");

            migrationBuilder.AddForeignKey(
                name: "FK_TripAnalytics_PeaksAnalytic_PeaksAnalyticsId",
                table: "TripAnalytics",
                column: "PeaksAnalyticsId",
                principalTable: "PeaksAnalytic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripAnalytics_PeaksAnalytic_PeaksAnalyticsId",
                table: "TripAnalytics");

            migrationBuilder.AddForeignKey(
                name: "FK_TripAnalytics_PeaksAnalytic_PeaksAnalyticsId",
                table: "TripAnalytics",
                column: "PeaksAnalyticsId",
                principalTable: "PeaksAnalytic",
                principalColumn: "Id");
        }
    }
}
