using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MadeElevationProfilEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeaksAnalytic_TripAnalytic_Id",
                table: "PeaksAnalytic");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_TripAnalytic_TripAnalyticId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripAnalytic",
                table: "TripAnalytic");

            migrationBuilder.DropColumn(
                name: "ElevationProfile_Name",
                table: "TripAnalytic");

            migrationBuilder.RenameTable(
                name: "TripAnalytic",
                newName: "TripAnalytics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripAnalytics",
                table: "TripAnalytics",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ElevationProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Start_Lat = table.Column<double>(type: "float", nullable: false),
                    Start_Lon = table.Column<double>(type: "float", nullable: false),
                    Start_Ele = table.Column<double>(type: "float", nullable: false),
                    Start_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GainsData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElevationProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElevationProfiles_TripAnalytics_Id",
                        column: x => x.Id,
                        principalTable: "TripAnalytics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PeaksAnalytic_TripAnalytics_Id",
                table: "PeaksAnalytic",
                column: "Id",
                principalTable: "TripAnalytics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_TripAnalytics_TripAnalyticId",
                table: "Trips",
                column: "TripAnalyticId",
                principalTable: "TripAnalytics",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeaksAnalytic_TripAnalytics_Id",
                table: "PeaksAnalytic");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_TripAnalytics_TripAnalyticId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "ElevationProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripAnalytics",
                table: "TripAnalytics");

            migrationBuilder.RenameTable(
                name: "TripAnalytics",
                newName: "TripAnalytic");

            migrationBuilder.AddColumn<string>(
                name: "ElevationProfile_Name",
                table: "TripAnalytic",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripAnalytic",
                table: "TripAnalytic",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PeaksAnalytic_TripAnalytic_Id",
                table: "PeaksAnalytic",
                column: "Id",
                principalTable: "TripAnalytic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_TripAnalytic_TripAnalyticId",
                table: "Trips",
                column: "TripAnalyticId",
                principalTable: "TripAnalytic",
                principalColumn: "Id");
        }
    }
}
