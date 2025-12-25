using Domain.Common.AggregateRoot;
using Domain.Common.Validations.Validators;
using Domain.FileReferences;
using Domain.Locations.Regions;
using Domain.Peaks;
using Domain.ReachedPeaks;
using Domain.ReachedPeaks.ValueObjects;
using Domain.Trips.Analytics.Peaks.Events;
using Domain.Trips.Analytics.Root;
using Domain.Trips.Analytics.Root.Extentions;
using Domain.Trips.Root.Events;
using Domain.Users.Root;

namespace Domain.Trips.Root;

public class Trip : AggregateRoot<Guid, Trip>
{
    public required string Name { get; set; }
    public DateOnly TripDay { get; private set; }

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
    public FileReference? GpxFile { get; set; }
    public TripAnalytic? Analytics { get; set; }
    #endregion

    public ICollection<ReachedPeak> Peaks { get; private set; } = [];

    public static Trip Create(Guid tripId, string name, DateOnly tripDay, Guid userId)
    {
        var trip = new Trip
        {
            Id = tripId,
            Name = name,
            UserId = userId,
            TripDay = tripDay,
        };

        return trip;
    }

    public Result<Trip> AddAnalytics(TripAnalytic analytic)
    {
        if (analytic == null)
        {
            return Errors.NotFound("passed null analytics");
        }
        Analytics = analytic;
        AddDomainEvent(new TripAnalyticsCreatedEvent(Id, UserId, analytic.ToStatUpdate()));
        return this;
    }

    public Result<Trip> AddReachedPeaks(CreateReachedPeak[] newPeaks)
    {
        if (newPeaks.Length == 0)
        {
            return Errors.EmptyCollection("new peaks");
        }

        AddDomainEvent(new ReachedNewPeak(new(UserId, Id, newPeaks)));

        return this;
    }

    public Result<Trip> AddReachedPeaks(List<ReachedPeak> newPeaks)
    {
        if (newPeaks.NullOrEmpty())
        {
            return Errors.EmptyCollection("new peaks");
        }

        foreach (var newPeak in newPeaks)
        {
            Peaks.Add(newPeak);
        }

        return this;
    }

    public void ChangeRegion(int regionID)
    {
        RegionId = regionID;
    }

    public void AddPeak(Peak peak)
    {
        ArgumentNullException.ThrowIfNull(peak);
        PeakId = peak.Id;
    }

    public Result<Trip> SetDate(DateOnly date)
    {
        var validTripDay = new DateOnlyValidator().NotInTheFuture().Validate(date);
        if (validTripDay.HasErrors(out var error))
        {
            return error;
        }

        TripDay = date;
        AddDomainEvent(new TripDateUpdatedEvent(this));
        return this;
    }

    public Trip AddGpxFile(FileReference gpxFile)
    {
        ArgumentNullException.ThrowIfNull(gpxFile);
        GpxFile = gpxFile;
        GpxFileId = gpxFile.Id;
        AddDomainEvent(new GpxFileAttatchedEvent(gpxFile.Id, Id));
        return this;
    }

    public Trip OnDelete(IList<ReachedPeak> tripsReached)
    {
        Console.WriteLine("Deleting");

        if (tripsReached.NotNullOrEmpty())
        {
            var peakUpdates = tripsReached
                .Select(rp => new PeakUpdateData(rp.PeakId, rp.Peak.RegionID))
                .ToArray();
            AddDomainEvent(new RemovedReachedPeaksEvent(UserId, peakUpdates));
        }

        AddDomainEvent(new TripRemovedEvent(this));
        return this;
    }
}
