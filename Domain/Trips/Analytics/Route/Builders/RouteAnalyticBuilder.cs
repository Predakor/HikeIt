using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Route;

namespace Domain.Trips.Analytics.Route.Builders;

public static class RouteAnalyticFactory {
    public static RouteAnalytic Create(AnalyticData data) {
        return RouteAnalyticsDirector.Complete(data.Points);
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
        _totalDescent = Math.Abs(_gains.Where(p => p.ElevationDelta < 0).Sum(p => p.ElevationDelta));
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
            TotalDistanceMeters = _totalDistance,
            TotalAscentMeters = _totalAscent,
            TotalDescentMeters = _totalDescent,

            HighestElevationMeters = _maxElevation,
            LowestElevationMeters = _minElevation,

            AverageSlopePercent = _averageSlope,
            AverageAscentSlopePercent = _averageAscentSlope,
            AverageDescentSlopePercent = _averageDescentSlope,
        };
    }
}
