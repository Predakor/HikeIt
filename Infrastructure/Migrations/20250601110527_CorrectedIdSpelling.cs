using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedIdSpelling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_GpxFiles_GpxFileID",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Regions_RegionID",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_GpxFileID",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "RegionID",
                table: "Trips",
                newName: "RegionId");

            migrationBuilder.RenameColumn(
                name: "GpxFileID",
                table: "Trips",
                newName: "GpxFileId");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_RegionID",
                table: "Trips",
                newName: "IX_Trips_RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_GpxFileId",
                table: "Trips",
                column: "GpxFileId",
                unique: true,
                filter: "[GpxFileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_GpxFiles_GpxFileId",
                table: "Trips",
                column: "GpxFileId",
                principalTable: "GpxFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Regions_RegionId",
                table: "Trips",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_GpxFiles_GpxFileId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Regions_RegionId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_GpxFileId",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "Trips",
                newName: "RegionID");

            migrationBuilder.RenameColumn(
                name: "GpxFileId",
                table: "Trips",
                newName: "GpxFileID");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_RegionId",
                table: "Trips",
                newName: "IX_Trips_RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_GpxFileID",
                table: "Trips",
                column: "GpxFileID",
                unique: true,
                filter: "[GpxFileID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_GpxFiles_GpxFileID",
                table: "Trips",
                column: "GpxFileID",
                principalTable: "GpxFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Regions_RegionID",
                table: "Trips",
                column: "RegionID",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
