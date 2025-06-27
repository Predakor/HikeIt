using Domain.Entiites.Peaks;
using Domain.Entiites.Regions;
using Domain.Entiites.Users;
using Domain.ReachedPeaks;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Domain.Trips;
using Domain.Trips.Entities.GpxFiles;
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

        modelBuilder.Entity<ReachedPeak>(entity => {
            entity
                .HasOne(rp => rp.Peak)
                .WithMany()
                .HasForeignKey(rp => rp.PeakId)
                .OnDelete(DeleteBehavior.NoAction);
            entity
                .HasOne(rp => rp.Trip)
                .WithOne()
                .HasForeignKey<ReachedPeak>(t => t.TripId)
                .OnDelete(DeleteBehavior.Cascade);
            entity
                .HasOne(rp => rp.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Trip>(entity => {
            entity
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity
                .HasOne(t => t.Region)
                .WithMany()
                .HasForeignKey(t => t.RegionId)
                .OnDelete(DeleteBehavior.NoAction);

            entity
                .HasOne(t => t.GpxFile)
                .WithOne()
                .HasForeignKey<Trip>(t => t.GpxFileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(t => t.Analytics)
                .WithOne()
                .HasForeignKey<TripAnalytic>(t => t.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TripAnalytic>(entity => {
            entity.OwnsOne(a => a.RouteAnalytics).WithOwner();
            entity.OwnsOne(a => a.TimeAnalytics).WithOwner();

            entity
                .HasOne(a => a.ElevationProfile)
                .WithOne()
                .HasForeignKey<ElevationProfile>(t => t.Id)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(a => a.PeaksAnalytic)
                .WithOne()
                .HasForeignKey<PeaksAnalytic>(t => t.Id)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PeaksAnalytic>(entity => {
            entity.OwnsOne(pa => pa.Summary).WithOwner();
        });

        modelBuilder.Entity<ElevationProfile>(e => {
            e.OwnsOne(e => e.Start).WithOwner();
        });
    }
}
