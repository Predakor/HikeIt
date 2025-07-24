using Domain.Common;
using Domain.Common.AggregateRoot;
using Domain.Common.Result;
using Domain.Interfaces;
using Domain.Mountains.Peaks;
using Domain.Mountains.Regions;
using Domain.TripAnalytics;
using Domain.Trips.Entities.GpxFiles;
using Domain.Users;
using Domain.Users.Extentions;
using Domain.Users.ValueObjects;

namespace Domain.Trips;

public record TripAnalyticsAddedDomainEvent(Guid UserId, StatsUpdates.All Summary) : IDomainEvent;

public class Trip : AggregateRoot<Guid>, IEntity<Guid> {
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

    public static Trip Create(Guid tripId, string name, DateOnly tripDay, Guid userId) {
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
        AddDomainEvent(new TripAnalyticsAddedDomainEvent(UserId, analytic.ToStatUpdate(TripDay)));
        return this;
    }

    public void ChangeRegion(int regionID) {
        RegionId = regionID;
    }

    public void AddPeak(Peak peak) {
        ArgumentNullException.ThrowIfNull(peak);
        PeakId = peak.Id;
    }

    public Trip AddGpxFile(GpxFile gpxFile) {
        ArgumentNullException.ThrowIfNull(gpxFile);
        GpxFileId = gpxFile.Id;
        return this;
    }
}
