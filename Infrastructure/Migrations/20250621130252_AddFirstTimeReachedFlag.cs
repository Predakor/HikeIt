using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstTimeReachedFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReachedPeaks_PeaksAnalytic_NewPeaksAnalyticId",
                table: "ReachedPeaks");

            migrationBuilder.DropForeignKey(
                name: "FK_ReachedPeaks_PeaksAnalytic_PeaksAnalyticId",
                table: "ReachedPeaks");

            migrationBuilder.DropIndex(
                name: "IX_ReachedPeaks_NewPeaksAnalyticId",
                table: "ReachedPeaks");

            migrationBuilder.DropIndex(
                name: "IX_ReachedPeaks_PeaksAnalyticId",
                table: "ReachedPeaks");

            migrationBuilder.DropColumn(
                name: "NewPeaksAnalyticId",
                table: "ReachedPeaks");

            migrationBuilder.DropColumn(
                name: "PeaksAnalyticId",
                table: "ReachedPeaks");

            migrationBuilder.AddColumn<bool>(
                name: "FirstTime",
                table: "ReachedPeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstTime",
                table: "ReachedPeaks");

            migrationBuilder.AddColumn<Guid>(
                name: "NewPeaksAnalyticId",
                table: "ReachedPeaks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PeaksAnalyticId",
                table: "ReachedPeaks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReachedPeaks_NewPeaksAnalyticId",
                table: "ReachedPeaks",
                column: "NewPeaksAnalyticId");

            migrationBuilder.CreateIndex(
                name: "IX_ReachedPeaks_PeaksAnalyticId",
                table: "ReachedPeaks",
                column: "PeaksAnalyticId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReachedPeaks_PeaksAnalytic_NewPeaksAnalyticId",
                table: "ReachedPeaks",
                column: "NewPeaksAnalyticId",
                principalTable: "PeaksAnalytic",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReachedPeaks_PeaksAnalytic_PeaksAnalyticId",
                table: "ReachedPeaks",
                column: "PeaksAnalyticId",
                principalTable: "PeaksAnalytic",
                principalColumn: "Id");
        }
    }
}
