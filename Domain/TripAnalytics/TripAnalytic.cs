using Domain.TripAnalytics.Builders.TripAnalyticBuilder;
using Domain.TripAnalytics.ValueObjects.PeaksAnalytics;
using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.TripAnalytics.ValueObjects.TimeAnalytics;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics;

public class TripAnalytic : IEntity<Guid> {
    public Guid Id { get; init; }

    #region Owned Types
    public RouteAnalytic? RouteAnalytics { get; init; }
    public TimeAnalytic? TimeAnalytics { get; init; }
    public PeaksAnalytic? PeaksAnalytics { get; init; }
    public ElevationProfile? ElevationProfile { get; init; }

    #endregion

    public static TripAnalytic Create(AnalyticData data) {
        return new TripAnalytic();
    }
}
