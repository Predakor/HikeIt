using Domain.Entiites.Peaks;
using Domain.Entiites.Regions;
using Domain.Entiites.Users;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Region>().HasData(DataSeed.Regions);

        modelBuilder.Entity<Peak>(builder => {
            builder.HasOne(p => p.Region).WithMany().HasForeignKey(p => p.RegionID);
            builder.HasData(DataSeed.Peaks);
        });

        modelBuilder.Entity<User>().HasData(DataSeed.Users);

        modelBuilder.Entity<Trip>(
            (builder) => {
                builder.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId);
                builder.HasOne(t => t.Region).WithMany().HasForeignKey(t => t.RegionId);
                builder.HasOne(t => t.GpxFile).WithOne().HasForeignKey<Trip>(t => t.GpxFileId);

                builder.OwnsOne(
                    t => t.TripAnalytics,
                    tripAnalytics => {
                        tripAnalytics.OwnsOne(a => a.TimeAnalytics);

                        tripAnalytics.OwnsMany(
                            a => a.ReachedPeaks,
                            rp => {
                                rp.HasOne(r => r.Peak)
                                    .WithMany()
                                    .HasForeignKey(r => r.PeakId)
                                    .IsRequired(false);

                                rp.OwnsOne(r => r.GpxPoint);
                            }
                        );
                    }
                );

                builder.HasData(DataSeed.Trips);
            }
        );
    }
}
