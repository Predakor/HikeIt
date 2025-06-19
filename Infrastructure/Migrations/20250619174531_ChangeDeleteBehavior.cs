using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReachedPeak_Trips_TripId",
                table: "ReachedPeak");

            migrationBuilder.AddForeignKey(
                name: "FK_ReachedPeak_Trips_TripId",
                table: "ReachedPeak",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReachedPeak_Trips_TripId",
                table: "ReachedPeak");

            migrationBuilder.AddForeignKey(
                name: "FK_ReachedPeak_Trips_TripId",
                table: "ReachedPeak",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id");
        }
    }
}
