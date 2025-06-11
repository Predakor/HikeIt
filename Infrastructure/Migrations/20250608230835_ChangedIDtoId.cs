using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedIDtoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_TripAnalytic_TripAnalyticID",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_TripAnalyticID",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "TripAnalyticID",
                table: "Trips",
                newName: "TripAnalyticId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TripAnalyticId",
                table: "Trips",
                column: "TripAnalyticId",
                unique: true,
                filter: "[TripAnalyticId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_TripAnalytic_TripAnalyticId",
                table: "Trips",
                column: "TripAnalyticId",
                principalTable: "TripAnalytic",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_TripAnalytic_TripAnalyticId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_TripAnalyticId",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "TripAnalyticId",
                table: "Trips",
                newName: "TripAnalyticID");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TripAnalyticID",
                table: "Trips",
                column: "TripAnalyticID",
                unique: true,
                filter: "[TripAnalyticID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_TripAnalytic_TripAnalyticID",
                table: "Trips",
                column: "TripAnalyticID",
                principalTable: "TripAnalytic",
                principalColumn: "Id");
        }
    }
}
