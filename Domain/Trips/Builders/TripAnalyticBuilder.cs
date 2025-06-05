using Domain.Trips.ValueObjects;
using Domain.Trips.ValueObjects.TripAnalytics;

namespace Domain.Trips.Builders;

public static class TripAnalyticDirector {
    public static TripAnalytic Create(GpxAnalyticData data) {
        return new TripAnalyticBuilder(data).GenerateGains().GenerateTripAnalytics().Build();
    }

    public static TripAnalytic CreateTimedAnalytics(GpxAnalyticData data) {
        return new TripAnalyticBuilder(data)
            .GenerateGains()
            .GenerateTripAnalytics()
            .GenerateTimeData()
            .Build();
    }
}

internal class TripAnalyticBuilder(GpxAnalyticData data) {
    List<GpxPoint> _gpxPoints = data.Data;
    TripTimeAnalytic _timeAnalytics;
    TripAnalytic _analytics;
    List<GpxGain> _gains;
    List<GpxPoint> _peaks;

    public TripAnalyticBuilder GenerateGains() {
        _gains = Methods.GenerateGains(_gpxPoints);
        return this;
    }

    public TripAnalyticBuilder GenerateTimeData() {
        var (pointsWithTime, gainsWithTime) = Methods.MapToTimed(_gpxPoints, _gains);

        if (gainsWithTime.Count == 0 || pointsWithTime.Count == 0) {
            return this;
        }

        _timeAnalytics = Methods.GenerateTimeAnalytic(gainsWithTime, pointsWithTime, _analytics);
        return this;
    }

    public TripAnalyticBuilder GenerateTripAnalytics() {
        _analytics = Methods.GenerateTripAnalytics(_gains, _gpxPoints);
        return this;
    }

    public TripAnalyticBuilder GeneratePeaks() {
        _peaks = Methods.FindPeaks(_gpxPoints, _gains);
        return this;
    }

    public TripAnalytic Build() {
        return _analytics;
    }

    internal static class Methods {
        public static (List<GpxPointWithTime>, List<GpxGainWithTime>) MapToTimed(
            List<GpxPoint> points,
            List<GpxGain> gains
        ) {
            var pointsWithTime = points
                .Where(p => p.Time != null)
                .Select(p => new GpxPointWithTime(p.Lat, p.Lon, p.Ele, (DateTime)p.Time!))
                .ToList();

            var gainsWithTime = gains
                .Where(p => p.TimeDelta != null)
                .Select(p => new GpxGainWithTime(
                    p.DistanceDelta,
                    p.ElevationDelta,
                    p.Slope,
                    (double)p.TimeDelta!
                ))
                .ToList();

            return (pointsWithTime, gainsWithTime);
        }

        public static List<GpxGain> GenerateGains(List<GpxPoint> data) {
            List<GpxGain> gains = new(data.Count - 1);

            for (int i = 1; i < data.Count; i++) {
                var current = data[i];
                var prev = data[i - 1];

                gains.Add(GpxPointHelpers.ComputeGain(current, prev));
            }

            return gains;
        }

        public static TripTimeAnalytic GenerateTimeAnalytic(
            List<GpxGainWithTime> gains,
            List<GpxPointWithTime> points,
            TripAnalytic analytics
        ) {
            DateTime start = points.First().Time;
            DateTime end = points.Last().Time;

            double iddleSpeedTreshold = 0.3d;
            double ascentElevationTreshold = 0.1d;

            double activeTime = gains
                .Where(g => g.TimeDelta >= iddleSpeedTreshold)
                .Sum(g => g.TimeDelta);

            double ascentTime = gains
                .Where(g => g.ElevationDelta > ascentElevationTreshold)
                .Sum(p => p.TimeDelta);

            double descentTime = gains
                .Where(g => g.ElevationDelta < -ascentElevationTreshold)
                .Sum(g => g.TimeDelta);

            TimeSpan duration = end - start;

            TimeSpan iddleTime = duration - TimeSpan.FromMicroseconds(activeTime);

            return new() {
                Duration = duration,
                StartTime = start,
                EndTime = end,

                IdleTime = iddleTime,
                ActiveTime = TimeSpan.FromMicroseconds(activeTime),
                AscentTime = TimeSpan.FromMicroseconds(ascentTime),
                DescentTime = TimeSpan.FromMicroseconds(descentTime),

                AverageAscentKph = analytics.TotalAscent / ascentTime,
                AverageDescentKph = analytics.TotalDescent / descentTime,
                AverageSpeedKph = analytics.TotalDistanceKm / activeTime,
            };
        }

        public static TripAnalytic GenerateTripAnalytics(List<GpxGain> gains, List<GpxPoint> points) {
            double totalDistance = gains.Sum(p => p.DistanceDelta);
            double totalAscent = gains.Where(p => p.ElevationDelta > 0).Sum(p => p.ElevationDelta);
            double totalDescent = gains.Where(p => p.ElevationDelta < 0).Sum(p => p.ElevationDelta);
            double maxElevation = points.Max(p => p.Ele);
            double minElevation = points.Min(p => p.Ele);

            return new TripAnalytic() {
                TotalDistanceKm = totalDistance,
                TotalAscent = totalAscent,
                TotalDescent = totalDescent,
                MinElevation = minElevation,
                MaxElevation = maxElevation,
            };
        }

        public static List<GpxPoint> FindPeaks(List<GpxPoint> data, List<GpxGain> gains) {
            //data is filtered from gps jitter and microjumps
            //so if ele delta gets smaller we probbalby are on local peak

            List<GpxPoint> localPeaks = new();

            for (int i = 0; i < gains.Count; i++) {
                //difference between gpxPoint[i] and gpxPoint[i-1]
                var eleDelta = gains[i].ElevationDelta;
                if (eleDelta > 0) {
                    continue;
                }

                localPeaks.Add(data[i]);
            }

            return localPeaks;
        }
    }
}
