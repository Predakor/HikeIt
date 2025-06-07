namespace Domain.Trips.ValueObjects;

public record GpxPoint(double Lat, double Lon, double Ele, DateTime? Time = null);

public record GpxPointWithTime(double Lat, double Lon, double Ele, DateTime Time);

public record GpxGain(short DistanceDelta, short ElevationDelta, short Slope, short? TimeDelta);

public record GpxGainWithTime(
    short DistanceDelta,
    short ElevationDelta,
    short Slope,
    short TimeDelta
);

public record TripAnalyticData(List<GpxPoint> Data);

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

    public static GpxGain ComputeGain(GpxPoint current, GpxPoint prev) {
        short planarDelta = (short)DistanceHelpers.Distance2D(current, prev);
        short eleDelta = (short)(current.Ele - prev.Ele);
        short timeDelta = (short)(current.Time - prev.Time)?.TotalSeconds;
        short slope = (short)(eleDelta / planarDelta * 100);

        return new GpxGain(planarDelta, eleDelta, slope, timeDelta);
    }

    public static (List<GpxPointWithTime>, List<GpxGainWithTime>) MapToTimed(
        List<GpxPoint> points,
        List<GpxGain> gains
    ) {
        var pointsWithTime = points.MapToTimed();
        var gainsWithTime = gains.MapToTimed();

        return (pointsWithTime, gainsWithTime);
    }

    public static List<GpxPointWithTime> MapToTimed(this List<GpxPoint> points) {
        return points
            .Where(p => p.Time != null)
            .Select(p => p.ToGpxWithTime((DateTime)p.Time!))
            .ToList();
    }

    public static List<GpxGainWithTime> MapToTimed(this List<GpxGain> gains) {
        return gains
            .Where(p => p.TimeDelta != null)
            .Select(p => p.ToGainWithTime((short)p.TimeDelta!))
            .ToList();
    }

    public static GpxPointWithTime ToGpxWithTime(this GpxPoint p, DateTime time) {
        return new GpxPointWithTime(p.Lat, p.Lon, p.Ele, time);
    }

    public static GpxGainWithTime ToGainWithTime(this GpxGain p, short timeDelta) {
        return new GpxGainWithTime(p.DistanceDelta, p.ElevationDelta, p.Slope, timeDelta);
    }

    public static double ToKph(this TimeSpan time, double distance) {
        return time.TotalHours > 0 ? distance / 1000 / time.TotalHours : 0;
    }
}

public static class DistanceHelpers {
    public static double Distance2D(GpxPoint p1, GpxPoint p2) {
        return HaversineDistance(p1, p2);
    }

    public static double Distance3D(GpxPoint p1, GpxPoint p2) {
        double planar = Distance2D(p1, p2);
        double elevationDiff = p2.Ele - p1.Ele;
        return Math.Sqrt((planar * planar) + (elevationDiff * elevationDiff));
    }

    public static double Distance3D(double planarDistance, double elevationDelta) {
        return Math.Sqrt(planarDistance * planarDistance + elevationDelta * elevationDelta);
    }

    private static double HaversineDistance(GpxPoint p1, GpxPoint p2) {
        const double R = 6371000; // Earth radius in meters

        double lat1Rad = DegreesToRadians(p1.Lat);
        double lat2Rad = DegreesToRadians(p2.Lat);
        double deltaLat = DegreesToRadians(p2.Lat - p1.Lat);
        double deltaLon = DegreesToRadians(p2.Lon - p1.Lon);

        double a =
            Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2)
            + Math.Cos(lat1Rad)
                * Math.Cos(lat2Rad)
                * Math.Sin(deltaLon / 2)
                * Math.Sin(deltaLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c;
    }

    private static double DegreesToRadians(double degrees) => degrees * Math.PI / 180;
}
