using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedGainsDataTypeToFloat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "RouteAnalytics_AverageSlope",
                table: "TripAnalytic",
                type: "real",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "RouteAnalytics_AverageDescentSlope",
                table: "TripAnalytic",
                type: "real",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "RouteAnalytics_AverageAscentSlope",
                table: "TripAnalytic",
                type: "real",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "RouteAnalytics_AverageSlope",
                table: "TripAnalytic",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "RouteAnalytics_AverageDescentSlope",
                table: "TripAnalytic",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "RouteAnalytics_AverageAscentSlope",
                table: "TripAnalytic",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
