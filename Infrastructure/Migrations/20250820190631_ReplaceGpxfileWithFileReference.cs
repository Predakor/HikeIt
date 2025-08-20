using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceGpxfileWithFileReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileReference_Trips_Id",
                table: "FileReference");

            migrationBuilder.DropTable(
                name: "GpxFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileReference",
                table: "FileReference");

            migrationBuilder.RenameTable(
                name: "FileReference",
                newName: "FileReferences");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileReferences",
                table: "FileReferences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileReferences_Trips_Id",
                table: "FileReferences",
                column: "Id",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileReferences_Trips_Id",
                table: "FileReferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileReferences",
                table: "FileReferences");

            migrationBuilder.RenameTable(
                name: "FileReferences",
                newName: "FileReference");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileReference",
                table: "FileReference",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GpxFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    OriginalName = table.Column<string>(type: "text", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpxFiles", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FileReference_Trips_Id",
                table: "FileReference",
                column: "Id",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
