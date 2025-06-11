namespace Domain.Trips.ValueObjects;

public abstract record GpxPointBase(double Lat, double Lon) {
    record GpxTimePoint(double Lat, double Lon, double Ele, DateTime Time) : GpxPointBase(Lat, Lon);

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

//scale 100x eg el gain of 0.01 -> 1
public readonly struct ScaledGain(float distanceDelta, float elevationDelta, float timeDelta) {
    readonly short _distanceDelta = (short)(distanceDelta * 100f);
    readonly short _elevationDelta = (short)(elevationDelta * 100f);
    readonly short _timeDelta = (short)timeDelta;

    public float DistanceDelta => _distanceDelta / 100f;
    public float ElevationDelta => _elevationDelta / 100f;
    public float TimeDelta => _timeDelta;
}

public record AnalyticData(List<GpxPoint> Data, List<GpxGain>? Gains = null);
