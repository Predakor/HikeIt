using Domain.Entiites.Peaks;
using Domain.Entiites.Regions;
using Domain.Entiites.Users;
using Domain.ReachedPeaks;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Domain.Trips;
using Domain.Trips.Entities.GpxFiles;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TripDbContext(DbContextOptions<TripDbContext> options) : DbContext(options) {
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Peak> Peaks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<GpxFile> GpxFiles { get; set; }
    public DbSet<TripAnalytic> TripAnalytics { get; set; }
    public DbSet<ElevationProfile> ElevationProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Region>().HasData(DataSeed.Regions);
        modelBuilder.Entity<User>().HasData(DataSeed.Users);

        modelBuilder.Entity<Peak>(builder => {
            builder.HasOne(p => p.Region).WithMany().HasForeignKey(p => p.RegionID);
            builder.HasData(DataSeed.Peaks);
        });

        modelBuilder.Entity<ReachedPeak>(builder => {
            builder
                .HasOne(rp => rp.Peak)
                .WithMany()
                .HasForeignKey(rp => rp.PeakId)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(rp => rp.Trip)
                .WithOne()
                .HasForeignKey<ReachedPeak>(t => t.TripId)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(rp => rp.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Trip>(builder => {
            builder
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(t => t.Region)
                .WithMany()
                .HasForeignKey(t => t.RegionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(t => t.GpxFile)
                .WithOne()
                .HasForeignKey<Trip>(t => t.GpxFileId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(t => t.Analytics)
                .WithOne()
                .HasForeignKey<Trip>(t => t.TripAnalyticId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasData(DataSeed.Trips);
        });

        modelBuilder.Entity<TripAnalytic>(builder => {
            builder.OwnsOne(a => a.RouteAnalytics).WithOwner();
            builder.OwnsOne(a => a.TimeAnalytics).WithOwner();

            builder
                .HasOne(a => a.ElevationProfile)
                .WithOne()
                .HasForeignKey<TripAnalytic>(t => t.ElevationProfileId)
                .IsRequired(false);

            builder
                .HasOne(a => a.PeaksAnalytic)
                .WithOne()
                .HasForeignKey<TripAnalytic>(t => t.PeaksAnalyticsId)
                .IsRequired(false);
        });

        modelBuilder.Entity<PeaksAnalytic>(builder => {
            builder
                .HasMany(pa => pa.ReachedPeaks)
                .WithOne() // No nav back on ReachedPeak
                .HasForeignKey("PeaksAnalyticId") // Explicit shadow FK
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(pa => pa.NewPeaks)
                .WithOne() // No nav back on ReachedPeak
                .HasForeignKey("NewPeaksAnalyticId") // Explicit shadow FK
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<ElevationProfile>().OwnsOne(ep => ep.Start).WithOwner();
    }
}
