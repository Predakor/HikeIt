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

public static class GpxPointHelpers {
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

    public static GpxGain ComputeGain(GpxPoint current, GpxPoint prev) {
        short planarDelta = (short)Distance2D(current, prev);
        short eleDelta = (short)(current.Ele - prev.Ele);
        short timeDelta = (short)(current.Time - prev.Time)?.TotalSeconds;
        short slope = (short)(eleDelta / planarDelta * 100);

        return new GpxGain(planarDelta, eleDelta, slope, timeDelta);
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
