using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.ElevationProfiles;
using Domain.Trips.Analytics.Peaks;
using Domain.Trips.Analytics.Route;
using Domain.Trips.Analytics.Time;

namespace Domain.Trips.Analytics.Root;

public class TripAnalyticBuilder
{
    #region mutable
    private Guid _id;
    private RouteAnalytic? _routeAnalytic;
    private TimeAnalytic? _timeAnalytic;
    private PeaksAnalytic? _peakAnalytic;
    private ElevationProfile? _elevationProfile;
    private RoutePath? _visualisation;

    #endregion

    public TripAnalyticBuilder WithId(Guid id)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(id));

        _id = id;
        return this;
    }

    public TripAnalyticBuilder WithRouteAnalytic(RouteAnalytic analytic)
    {
        ArgumentNullException.ThrowIfNull(analytic);

        _routeAnalytic = analytic;
        return this;
    }

    public TripAnalyticBuilder WithTimeAnalytic(TimeAnalytic analytic)
    {
        ArgumentNullException.ThrowIfNull(analytic);
        _timeAnalytic = analytic;
        return this;
    }

    public TripAnalyticBuilder WithPeaksAnalytic(PeaksAnalytic analytic)
    {
        ArgumentNullException.ThrowIfNull(analytic);
        _peakAnalytic = analytic;
        return this;
    }

    public TripAnalyticBuilder WithElevationProfile(ElevationProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        _elevationProfile = profile;
        return this;
    }

    public TripAnalyticBuilder WithVisualisation(RoutePath visualisation)
    {
        ArgumentNullException.ThrowIfNull(visualisation);
        _visualisation = visualisation;
        return this;
    }

    public TripAnalytic Build()
    {
        return TripAnalytic.Create(
            id: _id,
            routeAnalytics: _routeAnalytic,
            timeAnalytics: _timeAnalytic,
            peaksAnalytics: _peakAnalytic,
            elevationProfile: _elevationProfile,
            visualisationPath: _visualisation
        );
    }
}

internal class TripAnalyticDataValidator(AnalyticData data)
{
    private readonly List<GpxPoint> _data = data.Points;

    public bool Validate()
    {
        return _data.Count > 2;
    }

    public static bool ValidateData(AnalyticData data)
    {
        return new TripAnalyticDataValidator(data).Validate();
    }
}
