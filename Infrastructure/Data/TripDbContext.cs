using Domain.GpxFiles;
using Domain.Peaks;
using Domain.Regions;
using Domain.Trips;
using Domain.Users;
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
                builder.HasOne(t => t.Region).WithMany().HasForeignKey(t => t.RegionID);
                builder.HasOne(t => t.GpxFile).WithOne().HasForeignKey<Trip>(t => t.GpxFileID);
                builder.OwnsOne(
                    t => t.TrackAnalytics,
                    ta => ta.OwnsOne(a => a.TimeAnalytics).WithOwner()
                );

                builder.HasData(DataSeed.Trips);
            }
        );

    }
}
