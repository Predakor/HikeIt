using Domain.Common.AggregateRoot;
using Domain.FileReferences;
using Domain.Interfaces;
using Domain.Mountains.Regions;
using Domain.Peaks;
using Domain.ReachedPeaks;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Domain.Trips;
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
    public DbSet<FileReference> FileReferences { get; set; }
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
        var events = GatherEvents();
        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        await PublishEvents(events);
        return result;
    }

    List<IDomainEvent> GatherEvents() {
        return ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(e => e.Entity)
            .SelectMany(aggregate => {
                IReadOnlyCollection<IDomainEvent> events = [.. aggregate.Events];
                aggregate.ClearDomainEvents();
                return events;
            })
            .ToList();
    }

    async Task PublishEvents(List<IDomainEvent> domainEvents) {
        bool hasEvents = domainEvents.Count > 0;
        if (hasEvents) {
            Console.WriteLine(domainEvents.Count + " Events found dispatching");
            await domainEventDispatcher.DispatchAsync(domainEvents);
        }
    }
}
