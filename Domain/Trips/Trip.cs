using Domain.Common;
using Domain.Common.Result;
using Domain.Entiites.Peaks;
using Domain.Entiites.Regions;
using Domain.Entiites.Users;
using Domain.Interfaces;
using Domain.TripAnalytics;
using Domain.Trips.Entities.GpxFiles;

namespace Domain.Trips;

public class Trip : IEntity<Guid> {
    public Guid Id { get; init; }
    public required string Name { get; set; }
    public required DateOnly TripDay { get; set; }

    #region Foreign Keys

    public Guid UserId { get; private set; }
    public int RegionId { get; private set; }
    public int? PeakId { get; private set; }
    public TripAnalytic? Analytics { get; set; }
    public Guid? GpxFileId { get; private set; }
    #endregion

    #region Navigation Properties
    public User? User { get; set; }
    public Peak? Target { get; set; }
    public Region? Region { get; set; }
    public GpxFile? GpxFile { get; set; }
    #endregion

    public static Trip Create(
        Guid tripId,
        string name,
        DateOnly tripDay,
        Guid userId
    ) {
        var trip = new Trip {
            Id = tripId,
            Name = name,
            UserId = userId,
            TripDay = tripDay,
        };

        return trip;
    }

    public Result<Trip> AddAnalytics(TripAnalytic analytic) {
        if (analytic == null) {
            return Errors.NotFound("passed null analytics");
        }
        Analytics = analytic;
        return this;
    }

    public void ChangeRegion(int regionID) {
        RegionId = regionID;
    }

    public void AddPeak(Peak peak) {
        ArgumentNullException.ThrowIfNull(peak);
        PeakId = peak.Id;
    }

    public void AddGpxFile(Guid gpxFileId) {
        ArgumentNullException.ThrowIfNull(gpxFileId);
        GpxFileId = gpxFileId;
    }
}
