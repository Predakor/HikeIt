using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_GpxFiles_GpxFileId",
                table: "Trips");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_GpxFiles_GpxFileId",
                table: "Trips",
                column: "GpxFileId",
                principalTable: "GpxFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_GpxFiles_GpxFileId",
                table: "Trips");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_GpxFiles_GpxFileId",
                table: "Trips",
                column: "GpxFileId",
                principalTable: "GpxFiles",
                principalColumn: "Id");
        }
    }
}
