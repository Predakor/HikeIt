using Domain.Entiites.Users;
using Domain.Mountains.Peaks;
using Domain.Mountains.Regions;
using Domain.ReachedPeaks;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Domain.Trips;
using Domain.Trips.Entities.GpxFiles;
using Infrastructure.DI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TripDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid> {
    public TripDbContext(DbContextOptions<TripDbContext> options)
        : base(options) { }

    public DbSet<Trip> Trips { get; set; }
    public DbSet<Peak> Peaks { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<GpxFile> GpxFiles { get; set; }
    public DbSet<ReachedPeak> ReachedPeaks { get; set; }
    public DbSet<TripAnalytic> TripAnalytics { get; set; }
    public DbSet<ElevationProfile> ElevationProfiles { get; set; }
    public DbSet<PeaksAnalytic> PeaksAnalytics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyAggregatesConfigurations();
        modelBuilder.AllEntitiesToUtcTimes();
    }
}
