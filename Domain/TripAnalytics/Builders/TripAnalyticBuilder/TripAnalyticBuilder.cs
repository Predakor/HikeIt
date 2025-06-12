using Application.TripAnalytics.Services;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.TripAnalytics.ValueObjects.TimeAnalytics;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Builders.TripAnalyticBuilder;

public class TripAnalyticBuilder {
    readonly IElevationProfileService _elevationService;

    #region mutable
    RouteAnalytic? _routeAnalytic;
    TimeAnalytic? _timeAnalytic;
    PeaksAnalytic? _peakAnalytic;
    ElevationProfile? _elevationProfile;

    #endregion
    public TripAnalyticBuilder(IElevationProfileService service) {
        _elevationService = service;
    }

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

        //_elevationProfile;

        return TripAnalytic.Create(_routeAnalytic, _timeAnalytic, _peakAnalytic, _elevationProfile);
    }
}

internal class TripAnalyticDataValidator(AnalyticData data) {
    readonly List<GpxPoint> _data = data.Points;

    public bool Validate() {
        return _data.Count > 2;
    }

    public static bool ValidateData(AnalyticData data) {
        return new TripAnalyticDataValidator(data).Validate();
    }
}
