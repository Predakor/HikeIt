using Domain.Common.AggregateRoot;
using Domain.Entiites.Users;
using Domain.Interfaces;
using Domain.Mountains.Peaks;
using Domain.Mountains.Regions;
using Domain.ReachedPeaks;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Domain.Trips;
using Domain.Trips.Entities.GpxFiles;
using Domain.Users;
using Infrastructure.DI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TripDbContext(
    DbContextOptions<TripDbContext> options,
    IDomainEventDispatcher domainEventDispatcher
) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options) {
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

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    ) {
        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        await PublishDomainEventsAsync();
        return result;
    }

    async Task PublishDomainEventsAsync() {
        var domainEvents = ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(e => e.Entity)
            .SelectMany(aggregate => {
                IReadOnlyCollection<IDomainEvent> domainEvents = aggregate.Events;

                aggregate.ClearDomainEvents();
                return domainEvents;
            })
            .ToList();

        await domainEventDispatcher.DispatchAsync(domainEvents);
    }
}
