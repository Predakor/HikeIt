using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReachDistanceToReachedPeaks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeReached",
                table: "ReachedPeaks",
                newName: "ReachedAtTime");

            migrationBuilder.AddColumn<long>(
                name: "ReachedAtDistanceMeters",
                table: "ReachedPeaks",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReachedAtDistanceMeters",
                table: "ReachedPeaks");

            migrationBuilder.RenameColumn(
                name: "ReachedAtTime",
                table: "ReachedPeaks",
                newName: "TimeReached");
        }
    }
}
