using Domain.Common.Geography.ValueObjects;

namespace Domain.Common.Geography;

public static class DistanceHelpers {
    public static double Distance2D(IGeoPoint p1, IGeoPoint p2) {
        return HaversineDistance(p1.Lat, p1.Lon, p2.Lat, p2.Lon);
    }

    public static double Distance3D(IGeoPoint p1, IGeoPoint p2) {
        double planar = Distance2D(p1, p2);
        double elevationDiff = p2.Ele - p1.Ele;
        return Math.Sqrt(planar * planar + elevationDiff * elevationDiff);
    }

    public static double Distance3D(double planarDistance, double elevationDelta) {
        return Math.Sqrt(planarDistance * planarDistance + elevationDelta * elevationDelta);
    }

    private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2) {
        const double R = 6371000; // Earth radius in meters

        double lat1Rad = DegreesToRadians(lat1);
        double lat2Rad = DegreesToRadians(lat2);
        double deltaLat = DegreesToRadians(lat2 - lat1);
        double deltaLon = DegreesToRadians(lon2 - lon1);

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
