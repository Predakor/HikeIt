using Domain.Trips.ValueObjects;
using Domain.Trips.ValueObjects.TripAnalytics;

namespace Domain.Trips.Builders.TripTimeAnalyticBuilder;

public record TimeFrame(DateTime Start, DateTime End);

public record TimeAnalyticData(
    TripAnalytic Analytics,
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
        AscentTreshold = 0.2d,
        DescentTreshold = -0.3d,
        IdleSpeedTreshold = 0.3d,
    };

    public static TripTimeAnalytic Create(TimeAnalyticData data, TimeAnalyticConfig? config = null) {
        config ??= defaultConfig;

        return new TripTimeAnalyticBuilder(data, config)
            .WithTimeFrame()
            .WithAscentTime()
            .WithDescentTime()
            .WithClimbSpeeds()
            .Build();
    }
}

internal class TripTimeAnalyticBuilder(TimeAnalyticData data, TimeAnalyticConfig config) {
    readonly TimeAnalyticConfig _config = config;
    readonly TripAnalytic _analytics = data.Analytics;
    readonly List<GpxGainWithTime> _gains = data.Gains;
    readonly TimeFrame _timeFrame = data.TimeFrame;

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

    public TripTimeAnalyticBuilder WithTimeFrame() {
        StartTime = _timeFrame.Start;
        EndTime = _timeFrame.End;
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

    public TripTimeAnalyticBuilder WithClimbSpeeds() {
        AverageSpeedKph = ActiveTime.ToKph(_analytics.TotalDistanceKm);
        AverageAscentKph = AscentTime.ToKph(_analytics.TotalAscent);
        AverageDescentKph = DescentTime.ToKph(_analytics.TotalDescent);

        return this;
    }

    public TripTimeAnalyticBuilder WithActivityTime() {
        double activeTime = _gains
            .Where(g => g.TimeDelta >= _config.IdleSpeedTreshold)
            .Sum(g => g.TimeDelta);

        ActiveTime = TimeSpan.FromSeconds(activeTime);
        IdleTime = Duration - ActiveTime;

        return this;
    }

    public TripTimeAnalytic Build() {
        return new TripTimeAnalytic() {
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

static class Helpers {
    public static (List<GpxPointWithTime>, List<GpxGainWithTime>) MapToTimed(
        List<GpxPoint> points,
        List<GpxGain> gains
    ) {
        var pointsWithTime = points
            .Where(p => p.Time != null)
            .Select(p => p.ToGpxWithTIme((DateTime)p.Time!))
            .ToList();

        var gainsWithTime = gains
            .Where(p => p.TimeDelta != null)
            .Select(p => p.ToGainWithTime((double)p.TimeDelta!))
            .ToList();

        return (pointsWithTime, gainsWithTime);
    }

    public static GpxPointWithTime ToGpxWithTIme(this GpxPoint p, DateTime time) {
        return new GpxPointWithTime(p.Lat, p.Lon, p.Ele, time);
    }

    public static GpxGainWithTime ToGainWithTime(this GpxGain p, double timeDelta) {
        return new GpxGainWithTime(p.DistanceDelta, p.ElevationDelta, p.Slope, timeDelta);
    }

    public static double ToKph(this TimeSpan time, double distance) {
        return time.TotalHours > 0 ? (distance / 1000) / time.TotalHours : 0;
    }
}
