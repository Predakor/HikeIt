using Domain.Common;
using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Builders.RouteAnalyticsBuilder;

public static class RouteAnalyticFactory {
    public static RouteAnalytic Create(TripAnalyticData data) {
        return RouteAnalyticsDirector.Complete(data.Data);
    }

    public static RouteAnalytic Create(List<GpxPoint> points, List<GpxGain>? gains = null) {
        return RouteAnalyticsDirector.Complete(points, gains);
    }
}

public static class RouteAnalyticsDirector {
    public static RouteAnalytic Complete(List<GpxPoint> points, List<GpxGain>? gains = null) {
        return new RouteAnalyticsBuilder(points, gains ?? points.ToGains())
            .WithTotalDistance()
            .WithTotalAscent()
            .WithTotalDescent()
            .WithHighestPoint()
            .WithLowestPoint()
            .WithAverageSlope()
            .WithAverageDescentSlope()
            .WithAverageAscentSlope()
            .Build();
    }

    public static RouteAnalytic Basic(List<GpxPoint> points, List<GpxGain>? gains = null) {
        return new RouteAnalyticsBuilder(points, gains ?? points.ToGains())
            .WithTotalDistance()
            .WithTotalAscent()
            .WithTotalDescent()
            .WithHighestPoint()
            .WithLowestPoint()
            .Build();
    }
}

public class RouteAnalyticsBuilder(List<GpxPoint> points, List<GpxGain> gains) {
    readonly List<GpxPoint> _points = points;
    readonly List<GpxGain> _gains = gains;

    #region mutable
    double _totalDistance;
    double _totalAscent;
    double _totalDescent;

    double _minElevation;
    double _maxElevation;

    float _averageSlope;
    float _averageAscentSlope;
    float _averageDescentSlope;

    #endregion

    #region builder methods

    public RouteAnalyticsBuilder WithHighestPoint() {
        _maxElevation = _points.Max(p => p.Ele);
        return this;
    }

    public RouteAnalyticsBuilder WithLowestPoint() {
        _minElevation = _points.Min(p => p.Ele);
        return this;
    }

    public RouteAnalyticsBuilder WithTotalDistance() {
        _totalDistance = _gains.Sum(p => p.DistanceDelta);
        return this;
    }

    public RouteAnalyticsBuilder WithTotalDescent() {
        _totalDescent = _gains.Where(p => p.ElevationDelta < 0).Sum(p => p.ElevationDelta);
        return this;
    }

    public RouteAnalyticsBuilder WithTotalAscent() {
        _totalAscent = _gains.Where(p => p.ElevationDelta > 0).Sum(p => p.ElevationDelta);
        return this;
    }

    public RouteAnalyticsBuilder WithAverageSlope() {
        var avg = _gains.Average(p => p.Slope);
        _averageSlope = avg;
        return this;
    }

    public RouteAnalyticsBuilder WithAverageAscentSlope() {
        _averageAscentSlope = _gains.Where(p => p.Slope > 0).Average(p => p.Slope);
        return this;
    }

    public RouteAnalyticsBuilder WithAverageDescentSlope() {
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
