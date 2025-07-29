namespace Domain.Common.GeographyHelpers;

public static class GeoConstants {
    public const float EarthRadiusMeters = 6378137; // WGS84 radius
}

public static class GeoConverter {
    const float EarthRadius = GeoConstants.EarthRadiusMeters;
    const float PI = MathF.PI;

    public static float MetersToDegreesLatitude(float meters) {
        return meters / EarthRadius * (180 / PI);
    }

    public static float DegreesLatitudeToMeters(float degrees) {
        return (degrees * PI / 180) * EarthRadius;
    }

    public static float MetersToDegreesLongitude(float meters, float latitudeDegrees) {
        float latRad = latitudeDegrees * PI / 180;
        float radiusAtLat = EarthRadius * MathF.Cos(latRad);
        return (meters / radiusAtLat) * (180 / PI);
    }

    public static float DegreesLongitudeToMeters(float degrees, float latitudeDegrees) {
        float latRad = latitudeDegrees * PI / 180;
        float radiusAtLat = EarthRadius * MathF.Cos(latRad);
        return (degrees * PI / 180) * radiusAtLat;
    }
}
