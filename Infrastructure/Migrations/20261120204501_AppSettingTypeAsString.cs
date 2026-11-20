using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AppSettingTypeAsString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The old rows hold numeric ids; the startup seeder re-creates every setting from its code defaults.
            migrationBuilder.Sql("DELETE FROM \"AppSettings\";");

            migrationBuilder.AlterColumn<string>(
                name: "SettingType",
                table: "AppSettings",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Names cannot be cast back to ints; the seeder re-creates the rows.
            migrationBuilder.Sql("DELETE FROM \"AppSettings\";");

            migrationBuilder.AlterColumn<int>(
                name: "SettingType",
                table: "AppSettings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);
        }
    }
}
