using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReachedPeaks_TripId",
                table: "ReachedPeaks");

            migrationBuilder.CreateIndex(
                name: "IX_ReachedPeaks_TripId",
                table: "ReachedPeaks",
                column: "TripId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReachedPeaks_TripId",
                table: "ReachedPeaks");

            migrationBuilder.CreateIndex(
                name: "IX_ReachedPeaks_TripId",
                table: "ReachedPeaks",
                column: "TripId",
                unique: true);
        }
    }
}
