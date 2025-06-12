using Domain.Trips.ValueObjects;

namespace Domain.Common;

public static class GpxHelpers {
    public static List<GpxGain> ToGains(this List<GpxPoint> data) {
        if (data.Count < 2) {
            throw new Exception("passing gpx list with less than 2 points");
        }

        List<GpxGain> gains = new(data.Count - 1);

        for (int i = 1; i < data.Count; i++) {
            var current = data[i];
            var prev = data[i - 1];

            gains.Add(ComputeGain(current, prev));
        }

        return gains;
    }

    public static List<GpxGainWithTime> ToGains(this List<GpxPointWithTime> data) {
        if (data.Count < 2) {
            throw new Exception("passing gpx list with less than 2 points");
        }

        List<GpxGainWithTime> gains = new(data.Count - 1);

        for (int i = 1; i < data.Count; i++) {
            var current = data[i];
            var prev = data[i - 1];

            gains.Add(ComputeGain(current, prev));
        }

        return gains;
    }

    public static GpxGain ComputeGain(GpxPoint current, GpxPoint prev) {
        var (planarDelta, eleDelta) = GpxHelperMethods.GetDistanceDeltas(current, prev);
        float slope = GpxHelperMethods.GetSlope(planarDelta, eleDelta);
        float? timeDelta = GpxHelperMethods.TryGetTimeDelta(current.Time, prev.Time);

        return new GpxGain(planarDelta, eleDelta, slope, timeDelta);
    }

    public static GpxGainWithTime ComputeGain(GpxPointWithTime current, GpxPointWithTime prev) {
        var (planarDelta, eleDelta) = GpxHelperMethods.GetDistanceDeltas(current, prev);
        float slope = GpxHelperMethods.GetSlope(planarDelta, eleDelta);
        float timeDelta = (float)(current.Time - prev.Time).TotalSeconds;

        return new GpxGainWithTime(planarDelta, eleDelta, slope, timeDelta);
    }

    public static (List<GpxPointWithTime>, List<GpxGainWithTime>) MapToTimed(
        List<GpxPoint> points,
        List<GpxGain> gains
    ) {
        var pointsWithTime = points.MapToTimed();
        var gainsWithTime = gains.MapToTimed();

        return (pointsWithTime, gainsWithTime);
    }

    public static List<GpxPointWithTime> MapToTimed(this ICollection<GpxPoint> points) {
        return points
            .Where(p => p.Time != null)
            .Select(p => p.ToGpxWithTime((DateTime)p.Time!))
            .ToList();
    }

    public static List<GpxGainWithTime> MapToTimed(this ICollection<GpxGain> gains) {
        return gains
            .Where(p => p.TimeDelta != null)
            .Select(p => p.ToGainWithTime((float)p.TimeDelta!))
            .ToList();
    }

    public static List<ScaledGain> ToScaled(this ICollection<GpxGain> gains) {
        return gains.Select(g => g.ToScaled()).ToList();
    }

    public static GpxPointWithTime ToGpxWithTime(this GpxPoint p, DateTime time) {
        return new GpxPointWithTime(p.Lat, p.Lon, p.Ele, time);
    }

    public static GpxGainWithTime ToGainWithTime(this GpxGain p, float timeDelta) {
        return new GpxGainWithTime(p.DistanceDelta, p.ElevationDelta, p.Slope, timeDelta);
    }

    public static ScaledGain ToScaled(this GpxGain g) {
        return ScaledGainFactory.Create(g.DistanceDelta, g.ElevationDelta, g.TimeDelta ?? 0);
    }
}

internal static class GpxHelperMethods {
    public static (float distDelta, float eleDelta) GetDistanceDeltas(
        GpxPoint curr,
        GpxPoint prev
    ) {
        var planarDelta = (float)DistanceHelpers.Distance2D(curr, prev);
        var eleDelta = (float)(curr.Ele - prev.Ele);
        return (planarDelta, eleDelta);
    }

    public static (float distDelta, float eleDelta) GetDistanceDeltas(
        GpxPointWithTime curr,
        GpxPointWithTime prev
    ) {
        var planarDelta = (float)DistanceHelpers.Distance2D(curr, prev);
        var eleDelta = (float)(curr.Ele - prev.Ele);
        return (planarDelta, eleDelta);
    }

    public static float? TryGetTimeDelta(DateTime? currentTime, DateTime? prevTime) {
        return currentTime.HasValue && prevTime.HasValue
            ? (float)(currentTime.Value - prevTime.Value).TotalSeconds
            : null;
    }

    public static float GetSlope(double planarDelta, double eleDelta) {
        if (planarDelta == 0) {
            return 0;
        }
        double slope = eleDelta / planarDelta * 100;
        return (float)slope;
    }
}
