using Domain.Interfaces;
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


    public Guid PeaksAnalyticsId { get; private set; }
    public Guid ElevationProfileId { get; private set; }

    //navigation
    public PeaksAnalytic? PeaksAnalytic { get; set; }
    public ElevationProfile? ElevationProfile { get; set; }

    public static TripAnalytic Create(
        Guid id,
        RouteAnalytic? routeAnalytics,
        TimeAnalytic? timeAnalytics,
        PeaksAnalytic? peaksAnalytics,
        ElevationProfile? elevationProfile
    ) {
        var analytics = new TripAnalytic() {
            Id = id,
            RouteAnalytics = routeAnalytics,
            TimeAnalytics = timeAnalytics,
        };

        if (peaksAnalytics is not null) {
            analytics.AddPeaksAnalytic(peaksAnalytics);
        }

        if (elevationProfile is not null) {
            analytics.AddElevationProfile(elevationProfile);
        }

        return analytics;
    }

    public void AddPeaksAnalytic(PeaksAnalytic analytics) {
        ArgumentNullException.ThrowIfNull(analytics);
        PeaksAnalytic = analytics;
        PeaksAnalyticsId = Id;
    }

    public void AddElevationProfile(ElevationProfile profile) {
        ArgumentNullException.ThrowIfNull(profile);
        ElevationProfile = profile;
        ElevationProfileId = Id;
    }

    public void AddTimeAnalytics(TimeAnalytic analytics) {
        ArgumentNullException.ThrowIfNull(analytics);
        TimeAnalytics = analytics;
    }

    public void AddRouteAnalytics(RouteAnalytic analytics) {
        ArgumentNullException.ThrowIfNull(analytics);
        RouteAnalytics = analytics;
    }


}
