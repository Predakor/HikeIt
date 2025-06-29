using Domain.Entiites.Peaks;
using Domain.Entiites.Regions;
using Domain.Entiites.Users;
using Domain.ReachedPeaks;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.Trips;
using Domain.Trips.Entities.GpxFiles;
using Infrastructure.Data.EntitiesConfigurations;
using Infrastructure.Data.Seeding;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Region>().HasData(DataSeed.Regions);

        modelBuilder.Entity<Peak>(entity => {
            entity.HasOne(p => p.Region).WithMany().HasForeignKey(p => p.RegionID);
            entity.HasData(DataSeed.Peaks);
        });

        ConfigureTripsAggregate.Configure(modelBuilder);
    }
}
