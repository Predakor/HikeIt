using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReachedPeaksToTripAnalitics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReachedPeak",
                columns: table => new
                {
                    TripAnalyticTripId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GpxPoint_Lat = table.Column<double>(type: "float", nullable: false),
                    GpxPoint_Lon = table.Column<double>(type: "float", nullable: false),
                    GpxPoint_Ele = table.Column<double>(type: "float", nullable: false),
                    GpxPoint_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeReached = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PeakId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReachedPeak", x => new { x.TripAnalyticTripId, x.Id });
                    table.ForeignKey(
                        name: "FK_ReachedPeak_Peaks_PeakId",
                        column: x => x.PeakId,
                        principalTable: "Peaks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReachedPeak_Trips_TripAnalyticTripId",
                        column: x => x.TripAnalyticTripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReachedPeak_PeakId",
                table: "ReachedPeak",
                column: "PeakId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReachedPeak");
        }
    }
}
