using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.TripAnalytics.ValueObjects.TimeAnalytics;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Builders.TripTimeAnalyticBuilder;

public record TimeFrame(DateTime Start, DateTime End);

public record TimeAnalyticData(
    RouteAnalytic Analytics,
    List<GpxGainWithTime> Gains,
    TimeFrame TimeFrame
);

public record TimeAnalyticConfig() {
    public double AscentTreshold { get; init; }
    public double DescentTreshold { get; init; }
    public double IdleSpeedTreshold { get; init; }
};

public static class TimeAnalyticsDirector {
    static readonly TimeAnalyticConfig defaultConfig = new() {
        AscentTreshold = 0.1d,
        DescentTreshold = -0.2d,
        IdleSpeedTreshold = 0.5d,
    };

    public static TimeAnalytic Create(TimeAnalyticData data, TimeAnalyticConfig? config = null) {
        config ??= defaultConfig;

        return new TripTimeAnalyticBuilder(data, config)
            .WithTimeFrame(data.TimeFrame.Start, data.TimeFrame.End)
            .WithAscentTime()
            .WithDescentTime()
            .WithActivityTime()
            .WithClimbSpeeds(data.Analytics)
            .Build();
    }
}

internal class TripTimeAnalyticBuilder(TimeAnalyticData data, TimeAnalyticConfig config) {
    readonly TimeAnalyticConfig _config = config;
    readonly List<GpxGainWithTime> _gains = data.Gains;

    #region mutable stats

    //duration stats
    DateTime StartTime;
    DateTime EndTime;
    TimeSpan Duration;

    //activity time stats
    TimeSpan IdleTime;
    TimeSpan ActiveTime;
    TimeSpan AscentTime;
    TimeSpan DescentTime;

    //speed stats
    double AverageSpeedKph;
    double AverageAscentKph;
    double AverageDescentKph;

    #endregion

    public TripTimeAnalyticBuilder WithTimeFrame(DateTime start, DateTime end) {
        StartTime = start.ToUniversalTime();
        EndTime = end.ToUniversalTime();
        Duration = EndTime - StartTime;
        return this;
    }

    public TripTimeAnalyticBuilder WithAscentTime() {
        double ascentTime = _gains
            .Where(g => g.ElevationDelta > _config.AscentTreshold)
            .Sum(p => p.TimeDelta);

        AscentTime = TimeSpan.FromSeconds(ascentTime);

        return this;
    }

    public TripTimeAnalyticBuilder WithDescentTime() {
        double descentTime = _gains
            .Where(g => g.ElevationDelta < _config.DescentTreshold)
            .Sum(g => g.TimeDelta);

        DescentTime = TimeSpan.FromSeconds(descentTime);

        return this;
    }

    public TripTimeAnalyticBuilder WithClimbSpeeds(RouteAnalytic analytic) {
        AverageSpeedKph = ActiveTime.ToKph(analytic.TotalDistanceMeters);
        AverageAscentKph = AscentTime.ToKph(analytic.TotalAscentMeters);
        AverageDescentKph = DescentTime.ToKph(analytic.TotalDescentMeters);

        return this;
    }

    public TripTimeAnalyticBuilder WithActivityTime() {
        double activeTime = _gains
            .Where(g => {
                var kphSpeed = (g.DistanceDelta / g.TimeDelta) * 3.6;
                return kphSpeed >= _config.IdleSpeedTreshold;
            })
            .Sum(g => g.TimeDelta);

        ActiveTime = TimeSpan.FromSeconds(activeTime);
        IdleTime = Duration - ActiveTime;

        return this;
    }

    public TimeAnalytic Build() {
        return new TimeAnalytic() {
            StartTime = StartTime,
            EndTime = EndTime,
            Duration = Duration,

            ActiveTime = ActiveTime,
            IdleTime = IdleTime,
            AscentTime = AscentTime,
            DescentTime = DescentTime,

            AverageAscentKph = AverageAscentKph,
            AverageDescentKph = AverageDescentKph,
            AverageSpeedKph = AverageSpeedKph,
        };
    }
}

public static class Helpers {
    public static double ToKph(this TimeSpan time, double distance) {
        return time.TotalHours > 0 ? distance / 1000 / time.TotalHours : 0;
    }
}
