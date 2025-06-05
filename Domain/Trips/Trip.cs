using Domain.Entiites.Regions;
using Domain.Entiites.Users;
using Domain.Trips.Builders;
using Domain.Trips.Entities.GpxFiles;
using Domain.Trips.ValueObjects;
using Domain.Trips.ValueObjects.TripAnalytics;

namespace Domain.Trips;

public class Trip : IEntity<Guid> {
    readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public Guid Id { get; init; }
    public required float Height { get; set; }
    public required float Distance { get; set; }
    public required float Duration { get; set; }
    public required DateOnly TripDay { get; set; }

    //Owned Type
    public TripAnalytic? TripAnalytics { get; private set; }

    //Foreign Keys
    public int RegionId { get; set; }
    public Guid? GpxFileId { get; private set; }
    public Guid UserId { get; set; }

    //Navigation Property
    public Region? Region { get; }
    public GpxFile? GpxFile { get; }
    public User? User { get; }

    public static Trip Create(float height, float distance, float duration, DateOnly date) {
        return new() {
            Id = Guid.NewGuid(),
            Height = height,
            Distance = distance,
            Duration = duration,
            TripDay = date,
        };
    }

    public static Trip Create(
        float height,
        float distance,
        float duration,
        DateOnly date,
        GpxAnalyticData data
    ) {
        var gpxData = GpxDataBuilder.ProcessData(data);
        var analytics = TripAnalyticDirector.Create(gpxData);

        return new() {
            Id = Guid.NewGuid(),
            Height = height,
            Distance = distance,
            Duration = duration,
            TripDay = date,
            TripAnalytics = analytics,
        };
    }

    public void CreateAnalytic(GpxAnalyticData data) {
        var gpxData = GpxDataBuilder.ProcessData(data);
        var analytics = TripAnalyticDirector.Create(gpxData);

        AddAnalytics(analytics);
    }

    public void AddAnalytics(TripAnalytic analytic) {
        ArgumentNullException.ThrowIfNull(analytic);
        TripAnalytics = analytic;
    }

    public void ChangeRegion(int regionID) {
        RegionId = regionID;
    }

    public void AddGpxFile(Guid gpxFileId) {
        ArgumentNullException.ThrowIfNull(gpxFileId);
        GpxFileId = gpxFileId;
    }
}
