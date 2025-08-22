using Domain.Common.Abstractions;
using Domain.Common.AggregateRoot;
using Domain.FileReferences;
using Domain.Locations.Peaks;
using Domain.Locations.Regions;
using Domain.ReachedPeaks;
using Domain.Trips.Analytics.ElevationProfiles;
using Domain.Trips.Analytics.Peaks;
using Domain.Trips.Analytics.Root;
using Domain.Trips.Root;
using Domain.Users.Root;
using Infrastructure.Commons.Databases.Seeding;
using Infrastructure.Commons.DI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commons.Databases;

public class TripDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid> {
    readonly IEventPublisher _eventPublisher;

    public TripDbContext(DbContextOptions<TripDbContext> options, IEventPublisher eventPublisher)
        : base(options) {
        _eventPublisher = eventPublisher;
    }

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
        await _eventPublisher.PublishAsync(events, cancellationToken);
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
}
