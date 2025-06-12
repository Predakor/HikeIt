namespace Domain.Trips.ValueObjects;

public interface IGeoPoint {
    double Lat { get; }
    double Lon { get; }
    double Ele { get; }
}

public record GpxPoint(double Lat, double Lon, double Ele, DateTime? Time = null) : IGeoPoint;

public record GpxPointWithTime(double Lat, double Lon, double Ele, DateTime Time) : IGeoPoint;

public record AnalyticData(List<GpxPoint> Points, List<GpxGain>? Gains = null);
