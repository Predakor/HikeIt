using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DbRebase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TripAnalytic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RouteAnalytics_TotalDistanceKm = table.Column<double>(type: "float", nullable: true),
                    RouteAnalytics_TotalAscent = table.Column<double>(type: "float", nullable: true),
                    RouteAnalytics_TotalDescent = table.Column<double>(type: "float", nullable: true),
                    RouteAnalytics_HighestElevation = table.Column<double>(type: "float", nullable: true),
                    RouteAnalytics_LowestElevation = table.Column<double>(type: "float", nullable: true),
                    RouteAnalytics_AverageSlope = table.Column<short>(type: "smallint", nullable: true),
                    RouteAnalytics_AverageAscentSlope = table.Column<short>(type: "smallint", nullable: true),
                    RouteAnalytics_AverageDescentSlope = table.Column<short>(type: "smallint", nullable: true),
                    TimeAnalytics_Duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    TimeAnalytics_StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeAnalytics_EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeAnalytics_ActiveTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    TimeAnalytics_IdleTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    TimeAnalytics_AscentTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    TimeAnalytics_DescentTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    TimeAnalytics_AverageSpeedKph = table.Column<double>(type: "float", nullable: true),
                    TimeAnalytics_AverageAscentKph = table.Column<double>(type: "float", nullable: true),
                    TimeAnalytics_AverageDescentKph = table.Column<double>(type: "float", nullable: true),
                    ElevationProfile_Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripAnalytic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDay = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Peaks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peaks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Peaks_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeaksAnalytic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeaksAnalytic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeaksAnalytic_TripAnalytic_Id",
                        column: x => x.Id,
                        principalTable: "TripAnalytic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GpxFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpxFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GpxFiles_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TripDay = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    PeakId = table.Column<int>(type: "int", nullable: true),
                    TripAnalyticID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GpxFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_GpxFiles_GpxFileId",
                        column: x => x.GpxFileId,
                        principalTable: "GpxFiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trips_Peaks_PeakId",
                        column: x => x.PeakId,
                        principalTable: "Peaks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trips_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trips_TripAnalytic_TripAnalyticID",
                        column: x => x.TripAnalyticID,
                        principalTable: "TripAnalytic",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trips_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReachedPeak",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeReached = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeakId = table.Column<int>(type: "int", nullable: false),
                    NewPeaksAnalyticId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PeaksAnalyticId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReachedPeak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReachedPeak_PeaksAnalytic_NewPeaksAnalyticId",
                        column: x => x.NewPeaksAnalyticId,
                        principalTable: "PeaksAnalytic",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReachedPeak_PeaksAnalytic_PeaksAnalyticId",
                        column: x => x.PeaksAnalyticId,
                        principalTable: "PeaksAnalytic",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReachedPeak_Peaks_PeakId",
                        column: x => x.PeakId,
                        principalTable: "Peaks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReachedPeak_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReachedPeak_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Tatry" },
                    { 2, "Pieniny" },
                    { 3, "Beskid Śląski" },
                    { 4, "Beskid Żywiecki" },
                    { 5, "Beskid Mały" },
                    { 6, "Beskid Makowski" },
                    { 7, "Beskid Wyspowy" },
                    { 8, "Gorce" },
                    { 9, "Beskid Sądecki" },
                    { 10, "Beskid Niski" },
                    { 11, "Bieszczady" },
                    { 12, "Góry Świętokrzyskie" },
                    { 13, "Góry Sowie" },
                    { 14, "Góry Stołowe" },
                    { 15, "Góry Bystrzyckie" },
                    { 16, "Góry Orlickie" },
                    { 17, "Góry Bialskie" },
                    { 18, "Góry Złote" },
                    { 19, "Góry Opawskie" },
                    { 20, "Góry Bardzkie" },
                    { 21, "Masyw Śnieżnika" },
                    { 22, "Karkonosze" },
                    { 23, "Góry Izerskie" },
                    { 24, "Rudawy Janowickie" },
                    { 25, "Sudety Wałbrzyskie" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Email", "Name" },
                values: new object[,]
                {
                    { new Guid("183a96d7-9c20-4b18-b65b-d5d6676b57aa"), new DateOnly(1995, 8, 20), "kasia.wandziak@wp.pl", "Kasia" },
                    { new Guid("7a4f8c5b-19b7-4a6a-89c0-f9a2e98a9380"), new DateOnly(2002, 4, 15), "mistrzbiznesu@wp.pl", "Janusz" },
                    { new Guid("b91a0ed5-40a1-447e-8f48-c8d1e89c7c90"), new DateOnly(1990, 12, 11), "ewa.nowak@outlook.com", "Ewa" },
                    { new Guid("e5be7d3d-8320-4ef9-b60d-92b5464f2f1b"), new DateOnly(1988, 3, 2), "marek.kowalski@gmail.com", "Marek" }
                });

            migrationBuilder.InsertData(
                table: "Peaks",
                columns: new[] { "Id", "Height", "Name", "RegionID" },
                values: new object[,]
                {
                    { 1, 1603, "Śnieżka", 22 },
                    { 2, 1346, "Rysy", 1 },
                    { 3, 2050, "Giewont", 1 },
                    { 4, 1367, "Czupel", 3 }
                });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "Id", "GpxFileId", "Name", "PeakId", "RegionId", "TripAnalyticID", "TripDay", "UserId" },
                values: new object[,]
                {
                    { new Guid("b91a0ed5-40a1-447e-8f48-c8d1e89c7c91"), null, "Wycieczka na śnieżke", null, 22, null, new DateOnly(2023, 5, 1), new Guid("7a4f8c5b-19b7-4a6a-89c0-f9a2e98a9380") },
                    { new Guid("b91a0ed5-40a1-447e-8f48-c8d1e89c7c92"), null, "Śnieżne kotły", null, 22, null, new DateOnly(2025, 1, 16), new Guid("7a4f8c5b-19b7-4a6a-89c0-f9a2e98a9380") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GpxFiles_OwnerId",
                table: "GpxFiles",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Peaks_RegionID",
                table: "Peaks",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_ReachedPeak_NewPeaksAnalyticId",
                table: "ReachedPeak",
                column: "NewPeaksAnalyticId");

            migrationBuilder.CreateIndex(
                name: "IX_ReachedPeak_PeakId",
                table: "ReachedPeak",
                column: "PeakId");

            migrationBuilder.CreateIndex(
                name: "IX_ReachedPeak_PeaksAnalyticId",
                table: "ReachedPeak",
                column: "PeaksAnalyticId");

            migrationBuilder.CreateIndex(
                name: "IX_ReachedPeak_TripId",
                table: "ReachedPeak",
                column: "TripId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReachedPeak_UserId",
                table: "ReachedPeak",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_GpxFileId",
                table: "Trips",
                column: "GpxFileId",
                unique: true,
                filter: "[GpxFileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_PeakId",
                table: "Trips",
                column: "PeakId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_RegionId",
                table: "Trips",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TripAnalyticID",
                table: "Trips",
                column: "TripAnalyticID",
                unique: true,
                filter: "[TripAnalyticID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_UserId",
                table: "Trips",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReachedPeak");

            migrationBuilder.DropTable(
                name: "PeaksAnalytic");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "GpxFiles");

            migrationBuilder.DropTable(
                name: "Peaks");

            migrationBuilder.DropTable(
                name: "TripAnalytic");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
