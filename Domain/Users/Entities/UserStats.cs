using Domain.Interfaces;
using Domain.Users.Extentions;
using Domain.Users.ValueObjects;

namespace Domain.Users.Entities;

public class UserStats : IEntity<Guid> {
    public Guid Id { get; init; }

    //Totals
    public uint TotalTrips { get; private set; }
    public uint TotalDistanceM { get; private set; }
    public uint TotalAscentMeters { get; private set; }
    public uint TotalDescentMeters { get; private set; }
    public uint TotalPeaks { get; private set; }
    public TimeSpan TotalDuration { get; private set; }

    //Locations
    public uint UniquePeaks { get; private set; }
    public uint RegionsVisited { get; private set; }

    // probbably will add more in the future
    // like actual regions eg sudety
    //and change current regions to ranges eg tatry

    //Metas
    public DateOnly? FirstHikeDate { get; private set; }
    public DateOnly? LastHikeDate { get; private set; }
    public double LongestTripMeters { get; private set; }

    public void UpdateStats(StatsUpdates.All update, UpdateMode mode) {
        UpdateTripCount(mode);
        UpdateTotals(update.Totals, mode);
        UpdateLocations(update.Locations, mode);
        UpdateMetas(update.Metas);
    }

    public void UpdateTotals(StatsUpdates.Totals update, UpdateMode mode) {
        TotalDistanceM = TotalDistanceM.SafeUpdate(update.DistanceMeters, mode);
        TotalAscentMeters = TotalAscentMeters.SafeUpdate(update.AscentMeters, mode);
        TotalDescentMeters = TotalDescentMeters.SafeUpdate(update.DescentMeters, mode);
        TotalPeaks = TotalPeaks.SafeUpdate(update.Peaks, mode);
        TotalDuration = TotalDuration.SafeUpdate(update.Duration, mode);
    }

    public void UpdateMetas(StatsUpdates.Metas update) {
        if (FirstHikeDate == null || update.HikeDate < FirstHikeDate) {
            FirstHikeDate = update.HikeDate;
        }

        if (LastHikeDate == null || update.HikeDate > LastHikeDate) {
            LastHikeDate = update.HikeDate;
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
}
