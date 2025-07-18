using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class idk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeaksAnalytic_TripAnalytics_Id",
                table: "PeaksAnalytic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeaksAnalytic",
                table: "PeaksAnalytic");

            migrationBuilder.RenameTable(
                name: "PeaksAnalytic",
                newName: "PeaksAnalytics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeaksAnalytics",
                table: "PeaksAnalytics",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PeaksAnalytics_TripAnalytics_Id",
                table: "PeaksAnalytics",
                column: "Id",
                principalTable: "TripAnalytics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeaksAnalytics_TripAnalytics_Id",
                table: "PeaksAnalytics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeaksAnalytics",
                table: "PeaksAnalytics");

            migrationBuilder.RenameTable(
                name: "PeaksAnalytics",
                newName: "PeaksAnalytic");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeaksAnalytic",
                table: "PeaksAnalytic",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PeaksAnalytic_TripAnalytics_Id",
                table: "PeaksAnalytic",
                column: "Id",
                principalTable: "TripAnalytics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
