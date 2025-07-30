using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNavPropsForRegionProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RegionProgress_RegionId",
                table: "RegionProgress",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionProgress_UserId",
                table: "RegionProgress",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegionProgress_AspNetUsers_UserId",
                table: "RegionProgress",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegionProgress_Regions_RegionId",
                table: "RegionProgress",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegionProgress_AspNetUsers_UserId",
                table: "RegionProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_RegionProgress_Regions_RegionId",
                table: "RegionProgress");

            migrationBuilder.DropIndex(
                name: "IX_RegionProgress_RegionId",
                table: "RegionProgress");

            migrationBuilder.DropIndex(
                name: "IX_RegionProgress_UserId",
                table: "RegionProgress");
        }
    }
}
