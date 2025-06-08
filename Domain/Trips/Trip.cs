using Domain.Entiites.Peaks;
using Domain.Entiites.Regions;
using Domain.Entiites.Users;
using Domain.TripAnalytics;
using Domain.Trips.Entities.GpxFiles;

namespace Domain.Trips;

public class Trip : IEntity<Guid> {
    readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public Guid Id { get; init; }
    public required string Name { get; set; }
    public required DateOnly TripDay { get; set; }

    #region Foreign Keys

    public Guid UserId { get; private set; }
    public int RegionId { get; private set; }
    public int? PeakId { get; private set; }
    public Guid? TripAnalyticID { get; private set; }
    public Guid? GpxFileId { get; private set; }
    #endregion

    #region Navigation Properties
    public User? User { get; set; }
    public Peak? Target { get; set; }
    public Region? Region { get; set; }
    public GpxFile? GpxFile { get; set; }
    public TripAnalytic? Analytics { get; set; }
    #endregion

    public static Trip Create(
        string name,
        DateOnly tripDay,
        Guid userId,
        int? regionId = null,
        Guid? tripId = null
    ) {
        var trip = new Trip {
            Id = tripId ?? Guid.NewGuid(),
            Name = name,
            TripDay = tripDay,
            UserId = userId,
            RegionId = regionId ?? 0,
        };

        return trip;
    }

    public void AddAnalytics(TripAnalytic analytic) {
        ArgumentNullException.ThrowIfNull(analytic);
        TripAnalyticID = analytic.Id;
    }

    public void ChangeRegion(int regionID) {
        RegionId = regionID;
    }

    public void AddGpxFile(Guid gpxFileId) {
        ArgumentNullException.ThrowIfNull(gpxFileId);
        GpxFileId = gpxFileId;
    }
}
