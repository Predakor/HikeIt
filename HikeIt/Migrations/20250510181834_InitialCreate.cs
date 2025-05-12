using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HikeIt.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    Length = table.Column<float>(type: "real", nullable: false),
                    Duration = table.Column<float>(type: "real", nullable: false),
                    TripDay = table.Column<DateOnly>(type: "date", nullable: false),
                    RegionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
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
                table: "Trips",
                columns: new[] { "Id", "Duration", "Height", "Length", "RegionID", "TripDay" },
                values: new object[,]
                {
                    { new Guid("c7e2f35f-e7ee-487d-bbc7-2e6fbb7c8c8a"), 8f, 1000f, 23.7f, 1, new DateOnly(2020, 12, 1) },
                    { new Guid("c7e2f35f-e7ee-487d-bbc7-2e6fbb7c8c8b"), 4f, 620f, 14.2f, 22, new DateOnly(2023, 4, 7) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_RegionID",
                table: "Trips",
                column: "RegionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
