using Domain.Interfaces;
using Domain.Users.Extentions;
using Domain.Users.ValueObjects;

namespace Domain.Users.Entities;

public class UserStats : IEntity<Guid> {
    public Guid Id { get; init; }

    //Totals
    public uint TotalTrips { get; private set; }
    public uint TotalDistanceMeters { get; private set; }
    public uint TotalAscentMeters { get; private set; }
    public uint TotalDescentMeters { get; private set; }
    public TimeSpan TotalDuration { get; private set; }

    //Locations
    public uint TotalPeaks { get; private set; }
    public uint UniquePeaks { get; private set; }
    public uint RegionsVisited { get; private set; }

    //Metas
    public DateOnly? FirstHikeDate { get; private set; }
    public DateOnly? LastHikeDate { get; private set; }
    public uint LongestTripMeters { get; private set; }
    public TimeSpan LongestTripMinutes { get; private set; }

    public void UpdateStats(StatsUpdates.All update, UpdateMode mode) {
        //safeguard if some stats woulnd't zero out
        bool isLastTripToDelete = TotalTrips == 1 && mode == UpdateMode.Decrease;
        if (isLastTripToDelete) {
            Clear();
            return;
        }

        UpdateTripCount(mode);
        UpdateTotals(update.Totals, mode);
        UpdateLocations(update.Locations, mode);
        UpdateMetas(update.Metas);
    }

    public void UpdateTotals(StatsUpdates.Totals update, UpdateMode mode) {
        TotalDistanceMeters = TotalDistanceMeters.SafeUpdate(update.DistanceMeters, mode);
        TotalAscentMeters = TotalAscentMeters.SafeUpdate(update.AscentMeters, mode);
        TotalDescentMeters = TotalDescentMeters.SafeUpdate(update.DescentMeters, mode);
        TotalPeaks = TotalPeaks.SafeUpdate(update.Peaks, mode);
        TotalDuration = TotalDuration.SafeUpdate(update.Duration, mode);
    }

    public void UpdateFirstLastTripDate(DateOnly date) {
        if (FirstHikeDate is null) {
            FirstHikeDate = date;
            LastHikeDate = date;
            return;
        }

        if (date < FirstHikeDate) {
            FirstHikeDate = date;
            return;
        }

        if (date > LastHikeDate) {
            FirstHikeDate = date;
            return;
        }
    }

    public void UpdateMetas(StatsUpdates.Metas update) {
        if (update.Duration is not null && update.Duration > LongestTripMinutes) {
            LongestTripMinutes = update.Duration.Value;
        }

        if (update.DistanceMeters > LongestTripMeters) {
            LongestTripMeters = update.DistanceMeters;
        }
    }

    public void UpdateLocations(StatsUpdates.Locations update, UpdateMode mode) {
        UniquePeaks = UniquePeaks.SafeUpdate(update.UniquePeaks, mode);
        RegionsVisited = RegionsVisited.SafeUpdate(update.NewRegions, mode);
    }

    public void UpdateTripCount(UpdateMode mode, uint delta = 1) {
        TotalTrips = mode switch {
            UpdateMode.Increase => TotalTrips + delta,
            UpdateMode.Decrease => Math.Max(TotalTrips - delta, 0),
            UpdateMode.Set => delta,
            _ => TotalTrips,
        };
    }

    void Clear() {
        TotalTrips = 0;
        TotalDistanceMeters = 0;
        TotalAscentMeters = 0;
        TotalDescentMeters = 0;
        TotalPeaks = 0;
        TotalDuration = TimeSpan.Zero;

        UniquePeaks = 0;
        RegionsVisited = 0;

        FirstHikeDate = null;
        LastHikeDate = null;
        LongestTripMeters = 0;
    }
}
