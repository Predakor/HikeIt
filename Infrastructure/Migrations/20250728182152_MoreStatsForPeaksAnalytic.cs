using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoreStatsForPeaksAnalytic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary_TotalPeaks",
                table: "PeaksAnalytics");

            migrationBuilder.AddColumn<long>(
                name: "New",
                table: "PeaksAnalytics",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Total",
                table: "PeaksAnalytics",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Unique",
                table: "PeaksAnalytics",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "New",
                table: "PeaksAnalytics");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "PeaksAnalytics");

            migrationBuilder.DropColumn(
                name: "Unique",
                table: "PeaksAnalytics");

            migrationBuilder.AddColumn<int>(
                name: "Summary_TotalPeaks",
                table: "PeaksAnalytics",
                type: "integer",
                nullable: true);
        }
    }
}
