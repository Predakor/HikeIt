using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationsAndUpdateNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPeaks",
                table: "RegionProgress",
                newName: "UniqueReachedPeaks");

            migrationBuilder.RenameColumn(
                name: "ReachedPeaks",
                table: "RegionProgress",
                newName: "TotalReachedPeaks");

            migrationBuilder.AddColumn<short>(
                name: "TotalPeaksInRegion",
                table: "RegionProgress",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPeaksInRegion",
                table: "RegionProgress");

            migrationBuilder.RenameColumn(
                name: "UniqueReachedPeaks",
                table: "RegionProgress",
                newName: "TotalPeaks");

            migrationBuilder.RenameColumn(
                name: "TotalReachedPeaks",
                table: "RegionProgress",
                newName: "ReachedPeaks");
        }
    }
}
