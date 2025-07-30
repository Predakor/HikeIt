using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNavPropsForRegionProgressCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "RegionProgress",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegionProgress_UserId1",
                table: "RegionProgress",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RegionProgress_AspNetUsers_UserId1",
                table: "RegionProgress",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegionProgress_AspNetUsers_UserId1",
                table: "RegionProgress");

            migrationBuilder.DropIndex(
                name: "IX_RegionProgress_UserId1",
                table: "RegionProgress");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "RegionProgress");
        }
    }
}
