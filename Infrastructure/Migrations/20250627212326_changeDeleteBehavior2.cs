using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeDeleteBehavior2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElevationProfiles_TripAnalytics_Id",
                table: "ElevationProfiles");

            migrationBuilder.AddForeignKey(
                name: "FK_ElevationProfiles_TripAnalytics_Id",
                table: "ElevationProfiles",
                column: "Id",
                principalTable: "TripAnalytics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElevationProfiles_TripAnalytics_Id",
                table: "ElevationProfiles");

            migrationBuilder.AddForeignKey(
                name: "FK_ElevationProfiles_TripAnalytics_Id",
                table: "ElevationProfiles",
                column: "Id",
                principalTable: "TripAnalytics",
                principalColumn: "Id");
        }
    }
}
