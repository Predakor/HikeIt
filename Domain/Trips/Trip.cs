using Domain.Entiites.Regions;
using Domain.Entiites.Users;
using Domain.Trips.GpxFiles;
using Domain.Trips.TripAnalytics;

namespace Domain.Trips;

public class Trip : IEntity<Guid> {
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
