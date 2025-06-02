using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
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
                    Height = table.Column<float>(type: "real", nullable: false),
                    Distance = table.Column<float>(type: "real", nullable: false),
                    Duration = table.Column<float>(type: "real", nullable: false),
                    TripDay = table.Column<DateOnly>(type: "date", nullable: false),
                    TripAnalytics_TotalDistanceKm = table.Column<double>(type: "float", nullable: true),
                    TripAnalytics_TotalAscent = table.Column<double>(type: "float", nullable: true),
                    TripAnalytics_TotalDescent = table.Column<double>(type: "float", nullable: true),
                    TripAnalytics_MinElevation = table.Column<double>(type: "float", nullable: true),
                    TripAnalytics_MaxElevation = table.Column<double>(type: "float", nullable: true),
                    TripAnalytics_TimeAnalytics_Duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    TripAnalytics_TimeAnalytics_StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TripAnalytics_TimeAnalytics_EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TripAnalytics_TimeAnalytics_ActiveTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    TripAnalytics_TimeAnalytics_IdleTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    TripAnalytics_TimeAnalytics_AscentTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    TripAnalytics_TimeAnalytics_DescentTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    TripAnalytics_TimeAnalytics_AverageSpeedKph = table.Column<double>(type: "float", nullable: true),
                    TripAnalytics_TimeAnalytics_AverageAscentKph = table.Column<double>(type: "float", nullable: true),
                    TripAnalytics_TimeAnalytics_AverageDescentKph = table.Column<double>(type: "float", nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    GpxFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                        name: "FK_Trips_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                columns: new[] { "Id", "Distance", "Duration", "GpxFileId", "Height", "RegionId", "TripDay", "UserId" },
                values: new object[,]
                {
                    { new Guid("aa73d9ee-1b3d-4f0a-880d-6b2a4ea1d4e1"), 23.7f, 8f, null, 1000f, 1, new DateOnly(2020, 12, 1), new Guid("7a4f8c5b-19b7-4a6a-89c0-f9a2e98a9380") },
                    { new Guid("bfd29135-2469-4341-859f-41e42d59e0a3"), 14.2f, 4f, null, 620f, 22, new DateOnly(2023, 4, 7), new Guid("183a96d7-9c20-4b18-b65b-d5d6676b57aa") }
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
                name: "IX_Trips_GpxFileId",
                table: "Trips",
                column: "GpxFileId",
                unique: true,
                filter: "[GpxFileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_RegionId",
                table: "Trips",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_UserId",
                table: "Trips",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Peaks");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "GpxFiles");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
