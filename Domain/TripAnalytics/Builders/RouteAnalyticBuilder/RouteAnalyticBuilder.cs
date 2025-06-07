using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Builders.RouteAnalyticBuilder;

internal class RouteAnalyticBuilder(TripAnalyticData data, List<GpxGain> gains) {
    readonly List<GpxPoint> _points = data.Data;
    readonly List<GpxGain> _gains = gains;

    #region mutable
    double _totalDistance;
    double _totalAscent;
    double _totalDescent;

    double _minElevation;
    double _maxElevation;

    short _averageSlope;
    short _averageAscentSlope;
    short _averageDescentSlope;

    #endregion

    #region builder methods

    public RouteAnalyticBuilder WithHighestPoint() {
        _maxElevation = _points.Max(p => p.Ele);
        return this;
    }

    public RouteAnalyticBuilder WithLowestPoint() {
        _minElevation = _points.Min(p => p.Ele);
        return this;
    }

    public RouteAnalyticBuilder WithTotalDistance() {
        _totalDistance = _gains.Sum(p => p.DistanceDelta);
        return this;
    }

    public RouteAnalyticBuilder WithTotalDescent() {
        _totalDescent = _gains.Where(p => p.ElevationDelta < 0).Sum(p => p.ElevationDelta);
        return this;
    }

    public RouteAnalyticBuilder WithTotalAscent() {
        _totalAscent = _gains.Where(p => p.ElevationDelta > 0).Sum(p => p.ElevationDelta);
        return this;
    }

    public RouteAnalyticBuilder WithAverageSlope() {
        _averageSlope = (short)_gains.Average(p => p.Slope);
        return this;
    }

    public RouteAnalyticBuilder WithAverageAscentSlope() {
        _averageAscentSlope = (short)_gains.Where(p => p.Slope > 0).Average(p => p.Slope);
        return this;
    }

    public RouteAnalyticBuilder WithAverageDescentSlope() {
        _averageDescentSlope = (short)_gains.Where(p => p.Slope < 0).Average(p => p.Slope);
        return this;
    }

    #endregion

    public RouteAnalytic Build() {
        return new RouteAnalytic() {
            TotalDistanceKm = _totalDistance,
            TotalAscent = _totalAscent,
            TotalDescent = _totalDescent,

            HighestElevation = _maxElevation,
            LowestElevation = _minElevation,

            AverageSlope = _averageSlope,
            AverageAscentSlope = _averageAscentSlope,
            AverageDescentSlope = _averageDescentSlope,
        };
    }
}
