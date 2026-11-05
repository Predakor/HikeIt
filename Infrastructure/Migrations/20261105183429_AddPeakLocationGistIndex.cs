using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPeakLocationGistIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Peaks_Location",
                table: "Peaks",
                column: "Location")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.Sql("DELETE FROM \"AppSettings\";");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Peaks_Location",
                table: "Peaks");
        }
    }
}
