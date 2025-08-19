using Domain.Common;
using Domain.Common.AggregateRoot;
using Domain.Common.Result;
using Domain.Common.Utils;
using Domain.Common.Validations.Validators;
using Domain.Interfaces;
using Domain.Mountains.Regions;
using Domain.Peaks;
using Domain.ReachedPeaks;
using Domain.ReachedPeaks.ValueObjects;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Events;
using Domain.Trips.Entities.GpxFiles;
using Domain.Trips.Events;
using Domain.Users;
using Domain.Users.Extentions;

namespace Domain.Trips;

public class Trip : AggregateRoot<Guid>, IEntity<Guid> {
    public required string Name { get; set; }
    public DateOnly TripDay { get; private set; } = default;

    #region Foreign Keys

    public Guid UserId { get; private set; }
    public int RegionId { get; private set; }
    public int? PeakId { get; private set; }
    public Guid? GpxFileId { get; private set; }
    #endregion

    #region Navigation Properties
    public User? User { get; set; }
    public Peak? Target { get; set; }
    public Region? Region { get; set; }
    public GpxFile? GpxFile { get; set; }
    public TripAnalytic? Analytics { get; set; }
    #endregion

    public ICollection<ReachedPeak> Peaks { get; private set; } = [];

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
        AddDomainEvent(new TripAnalyticsCreatedEvent(this, analytic.ToStatUpdate(TripDay)));
        return this;
    }

    public Result<Trip> AddReachedPeaks(ReachedPeakData[] newPeaks) {
        if (newPeaks.Length == 0) {
            return Errors.EmptyCollection("new peaks");
        }

        AddDomainEvent(new UserReachedNewPeaksEvent(new(UserId, Id, newPeaks)));

        return this;
    }

    public Result<Trip> AddReachedPeaks(List<ReachedPeak> newPeaks) {
        if (newPeaks.Count == 0) {
            return Errors.EmptyCollection("new peaks");
        }

        foreach (var newPeak in newPeaks) {
            Peaks.Add(newPeak);
        }

        return this;
    }

    public void ChangeRegion(int regionID) {
        RegionId = regionID;
    }

    public void AddPeak(Peak peak) {
        ArgumentNullException.ThrowIfNull(peak);
        PeakId = peak.Id;
    }

    public void SetDate(DateOnly date) {
        var validTripDay = new DateOnlyValidator().NotInTheFuture().Validate(date);

        if (validTripDay.IsSuccess) {
            TripDay = date;
        }
    }

    public Trip AddGpxFile(GpxFile gpxFile) {
        ArgumentNullException.ThrowIfNull(gpxFile);
        GpxFile = gpxFile;
        GpxFileId = gpxFile.Id;
        return this;
    }

    public Trip OnDelete(IList<ReachedPeak> tripsReached) {
        Console.WriteLine("Deleting");

        if (tripsReached.NotNullOrEmpty()) {
            var peakUpdates = tripsReached
                .Select(rp => new PeakUpdateData(rp.PeakId, rp.Peak.RegionID))
                .ToArray();
            AddDomainEvent(new ReachedPeakRemovedEvent(UserId, peakUpdates));
        }

        AddDomainEvent(new TripRemovedEvent(this));
        return this;
    }
}
