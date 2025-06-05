using Domain.Trips.ValueObjects;
using Domain.Trips.ValueObjects.TripAnalytics;

namespace Domain.Trips.Builders.TripAnalyticBuilder;

internal class TripAnalyticBuilder(GpxAnalyticData data) {
    readonly List<GpxPoint> _points = data.Data;

    #region mutable
    List<GpxGain> _gains = [];
    List<ReachedPeak> _peaks = [];

    double _totalDistance;
    double _totalAscent;
    double _totalDescent;
    double _minElevation;
    double _maxElevation;

    TripTimeAnalytic _timeAnalytic;
    #endregion

    public TripAnalyticBuilder WithGains() {
        _gains = TripBuilderMethods.GenerateGains(_points);
        return this;
    }

    public TripAnalyticBuilder WithHighestPoint() {
        _maxElevation = _points.Max(p => p.Ele);
        return this;
    }

    public TripAnalyticBuilder WithLowestPoint() {
        _minElevation = _points.Min(p => p.Ele);
        return this;
    }

    public TripAnalyticBuilder WithTotalDistance() {
        _totalDistance = _gains.Sum(p => p.DistanceDelta);
        return this;
    }

    public TripAnalyticBuilder WithTotalDescent() {
        _totalDescent = _gains.Where(p => p.ElevationDelta < 0).Sum(p => p.ElevationDelta);
        return this;
    }

    public TripAnalyticBuilder WithTotalAscent() {
        _totalAscent = _gains.Where(p => p.ElevationDelta > 0).Sum(p => p.ElevationDelta);
        return this;
    }

    public TripAnalyticBuilder WithClimbedPeaks() {
        _peaks = TripBuilderMethods
            .FindLocalPeaks(_points, _gains)
            .Select(p => new ReachedPeak() { GpxPoint = p, TimeReached = p?.Time })
            .ToList();

        return this;
    }

    public TripAnalyticBuilder WithTimeAnalytic(TripTimeAnalytic timeAnalytic) {
        _timeAnalytic = timeAnalytic;
        return this;
    }

    public TripAnalytic Build() {
        return new TripAnalytic() {
            TotalDistanceKm = _totalDistance,
            TotalAscent = _totalAscent,
            TotalDescent = _totalDescent,
            MinElevation = _minElevation,
            MaxElevation = _maxElevation,
            TimeAnalytics = _timeAnalytic ?? null,
            ReachedPeaks = _peaks ?? null,
        };
    }
}
