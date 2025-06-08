using Domain.TripAnalytics.ValueObjects.PeaksAnalytics;
using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.TripAnalytics.ValueObjects.TimeAnalytics;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Builders.TripAnalyticBuilder;

public class TripAnalyticBuilder {
    #region mutable
    RouteAnalytic _routeAnalytic;
    TimeAnalytic _timeAnalytic;
    PeaksAnalytic _peakAnalytic;
    ElevationProfile _elevationProfile;

    #endregion


    public TripAnalyticBuilder WithRouteAnalytic(RouteAnalytic analytic) {
        ArgumentException.ThrowIfNullOrEmpty(nameof(analytic));

        _routeAnalytic = analytic;
        return this;
    }

    public TripAnalyticBuilder WithTimeAnalytic(TimeAnalytic analytic) {
        ArgumentException.ThrowIfNullOrEmpty(nameof(analytic));

        _timeAnalytic = analytic;
        return this;
    }

    public TripAnalyticBuilder WithPeaksAnalytic(PeaksAnalytic analytic) {
        ArgumentException.ThrowIfNullOrEmpty(nameof(analytic));

        _peakAnalytic = analytic;
        return this;
    }

    public TripAnalyticBuilder WithElevationProfile(ElevationProfile profile) {
        ArgumentException.ThrowIfNullOrEmpty(nameof(profile));

        _elevationProfile = profile;
        return this;
    }

    public TripAnalytic Build() {
        return new() {
            Id = Guid.NewGuid(),
            RouteAnalytics = _routeAnalytic,
            TimeAnalytics = _timeAnalytic,
            PeaksAnalytics = _peakAnalytic,
            ElevationProfile = _elevationProfile,
        };
    }
}

public class ElevationProfile {
    public required string Name { get; set; }
}

internal class TripAnalyticDataValidator(TripAnalyticData data) {
    readonly List<GpxPoint> _data = data.Data;

    public bool Validate() {
        return _data.Count > 2;
    }

    public static bool ValidateData(TripAnalyticData data) {
        return new TripAnalyticDataValidator(data).Validate();
    }
}
