namespace Domain.Trips.ValueObjects;

public abstract record GpxPointBase(double Lat, double Lon) {
    record GpxTimePoint(double Lat, double Lon, double Ele, DateTime Time)
       : GpxPointBase(Lat, Lon);

    record GpxPoint(double Lat, double Lon, double Ele, DateTime? Time = null)
       : GpxPointBase(Lat, Lon);
}

public record GpxPoint(double Lat, double Lon, double Ele, DateTime? Time = null);

public record GpxPointWithTime(double Lat, double Lon, double Ele, DateTime Time);

public record GpxGain(float DistanceDelta, float ElevationDelta, float Slope, float? TimeDelta);

public record GpxGainWithTime(
    float DistanceDelta,
    float ElevationDelta,
    float Slope,
    float TimeDelta
);

public record TripAnalyticData(List<GpxPoint> Data);
