using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.TripAnalytics.ValueObjects.TimeAnalytics;

namespace Domain.TripAnalytics;

public class TripAnalytic : IEntity<Guid> {
    public Guid Id { get; init; }

    #region Owned Types
    public RouteAnalytic? RouteAnalytics { get; private set; }
    public TimeAnalytic? TimeAnalytics { get; private set; }
    #endregion

    #region Foreign types
    public PeaksAnalytic? PeaksAnalytics { get; private set; }
    public ElevationProfile? ElevationProfile { get; private set; }

    public static TripAnalytic Create(
        RouteAnalytic? routeAnalytics,
        TimeAnalytic? timeAnalytics,
        PeaksAnalytic? peaksAnalytics,
        ElevationProfile? elevationProfile
    ) {
        return new TripAnalytic() {
            RouteAnalytics = routeAnalytics,
            TimeAnalytics = timeAnalytics,
            PeaksAnalytics = peaksAnalytics,
            ElevationProfile = elevationProfile,
        };
    }

    public void AddPeaksAnalytic(PeaksAnalytic analytics) {
        ArgumentNullException.ThrowIfNull(analytics);
        PeaksAnalytics = analytics;
    }

    public void AddTimeAnalytics(TimeAnalytic analytics) {
        ArgumentNullException.ThrowIfNull(analytics);
        TimeAnalytics = analytics;
    }

    public void AddRouteAnalytics(RouteAnalytic analytics) {
        ArgumentNullException.ThrowIfNull(analytics);
        RouteAnalytics = analytics;
    }

    public void AddElevationProfile(ElevationProfile profile) {
        ArgumentNullException.ThrowIfNull(profile);
        ElevationProfile = profile;
    }



    #endregion
}
