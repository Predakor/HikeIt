using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoreCollumnNameWithUnits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalDistanceM",
                table: "UserStats",
                newName: "TotalDistanceMeters");

            migrationBuilder.AlterColumn<long>(
                name: "LongestTripMeters",
                table: "UserStats",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalDistanceMeters",
                table: "UserStats",
                newName: "TotalDistanceM");

            migrationBuilder.AlterColumn<double>(
                name: "LongestTripMeters",
                table: "UserStats",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
