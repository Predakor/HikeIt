using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations {
    /// <inheritdoc />
    public partial class init : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    BirthDay = table.Column<DateOnly>(type: "date", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStats",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalTrips = table.Column<long>(type: "bigint", nullable: false),
                    TotalDistanceMeters = table.Column<long>(type: "bigint", nullable: false),
                    TotalAscentMeters = table.Column<long>(type: "bigint", nullable: false),
                    TotalDescentMeters = table.Column<long>(type: "bigint", nullable: false),
                    TotalPeaks = table.Column<long>(type: "bigint", nullable: false),
                    TotalDuration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    UniquePeaks = table.Column<long>(type: "bigint", nullable: false),
                    RegionsVisited = table.Column<long>(type: "bigint", nullable: false),
                    FirstHikeDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LastHikeDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LongestTripMeters = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_UserStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStats_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Peaks",
                columns: table => new {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<Point>(type: "geography (Point, 4326)", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RegionID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Peaks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Peaks_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegionProgress",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegionId = table.Column<int>(type: "integer", nullable: false),
                    TotalPeaksInRegion = table.Column<short>(type: "smallint", nullable: false),
                    TotalReachedPeaks = table.Column<short>(type: "smallint", nullable: false),
                    UniqueReachedPeaks = table.Column<short>(type: "smallint", nullable: false),
                    PeakVisits = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_RegionProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegionProgress_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionProgress_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TripDay = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegionId = table.Column<int>(type: "integer", nullable: false),
                    PeakId = table.Column<int>(type: "integer", nullable: true),
                    GpxFileId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trips_Peaks_PeakId",
                        column: x => x.PeakId,
                        principalTable: "Peaks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trips_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GpxFiles",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false),
                    OriginalName = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_GpxFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GpxFiles_Trips_Id",
                        column: x => x.Id,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReachedPeaks",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstTime = table.Column<bool>(type: "boolean", nullable: false),
                    ReachedAtTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReachedAtDistanceMeters = table.Column<long>(type: "bigint", nullable: true),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PeakId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_ReachedPeaks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReachedPeaks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReachedPeaks_Peaks_PeakId",
                        column: x => x.PeakId,
                        principalTable: "Peaks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReachedPeaks_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripAnalytics",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteAnalytics_TotalDistanceMeters = table.Column<double>(type: "double precision", nullable: true),
                    RouteAnalytics_TotalAscentMeters = table.Column<double>(type: "double precision", nullable: true),
                    RouteAnalytics_TotalDescentMeters = table.Column<double>(type: "double precision", nullable: true),
                    RouteAnalytics_HighestElevationMeters = table.Column<double>(type: "double precision", nullable: true),
                    RouteAnalytics_LowestElevationMeters = table.Column<double>(type: "double precision", nullable: true),
                    RouteAnalytics_AverageSlopePercent = table.Column<float>(type: "real", nullable: true),
                    RouteAnalytics_AverageAscentSlopePercent = table.Column<float>(type: "real", nullable: true),
                    RouteAnalytics_AverageDescentSlopePercent = table.Column<float>(type: "real", nullable: true),
                    TimeAnalytics_Duration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    TimeAnalytics_StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TimeAnalytics_EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TimeAnalytics_ActiveTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    TimeAnalytics_IdleTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    TimeAnalytics_AscentTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    TimeAnalytics_DescentTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    TimeAnalytics_AverageSpeedKph = table.Column<double>(type: "double precision", nullable: true),
                    TimeAnalytics_AverageAscentKph = table.Column<double>(type: "double precision", nullable: true),
                    TimeAnalytics_AverageDescentKph = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_TripAnalytics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripAnalytics_Trips_Id",
                        column: x => x.Id,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElevationProfiles",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Start_Lat = table.Column<double>(type: "double precision", nullable: false),
                    Start_Lon = table.Column<double>(type: "double precision", nullable: false),
                    Start_Ele = table.Column<double>(type: "double precision", nullable: false),
                    Start_Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GainsData = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_ElevationProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElevationProfiles_TripAnalytics_Id",
                        column: x => x.Id,
                        principalTable: "TripAnalytics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeaksAnalytics",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Total = table.Column<long>(type: "bigint", nullable: false),
                    Unique = table.Column<long>(type: "bigint", nullable: false),
                    New = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PeaksAnalytics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeaksAnalytics_TripAnalytics_Id",
                        column: x => x.Id,
                        principalTable: "TripAnalytics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Peaks_RegionID",
                table: "Peaks",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_ReachedPeaks_PeakId",
                table: "ReachedPeaks",
                column: "PeakId");

            migrationBuilder.CreateIndex(
                name: "IX_ReachedPeaks_TripId",
                table: "ReachedPeaks",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_ReachedPeaks_UserId",
                table: "ReachedPeaks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionProgress_RegionId",
                table: "RegionProgress",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionProgress_UserId",
                table: "RegionProgress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_PeakId",
                table: "Trips",
                column: "PeakId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_RegionId",
                table: "Trips",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_UserId",
                table: "Trips",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ElevationProfiles");

            migrationBuilder.DropTable(
                name: "GpxFiles");

            migrationBuilder.DropTable(
                name: "PeaksAnalytics");

            migrationBuilder.DropTable(
                name: "ReachedPeaks");

            migrationBuilder.DropTable(
                name: "RegionProgress");

            migrationBuilder.DropTable(
                name: "UserStats");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "TripAnalytics");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Peaks");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
