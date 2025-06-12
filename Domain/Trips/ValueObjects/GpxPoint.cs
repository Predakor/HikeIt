namespace Domain.Trips.ValueObjects;

public abstract record GpxPointBase(double Lat, double Lon) {
    record GpxTimePoint(double Lat, double Lon, double Ele, DateTime Time) : GpxPointBase(Lat, Lon);

    record GpxPoint(double Lat, double Lon, double Ele, DateTime? Time = null)
        : GpxPointBase(Lat, Lon);
}

public record GpxPoint(double Lat, double Lon, double Ele, DateTime? Time = null);

public record GpxPointWithTime(double Lat, double Lon, double Ele, DateTime Time);

public record GpxGain(
    float DistanceDelta,
    float ElevationDelta,
    float Slope,
    float? TimeDelta = null
);

public record GpxGainWithTime(
    float DistanceDelta,
    float ElevationDelta,
    float Slope,
    float TimeDelta
);

public static class ScaledGainFactory {
    public static ScaledGain Create(float distanceDelta, float elevationDelta, float timeDelta) {
        return new ScaledGain(
            (short)(distanceDelta * 100f),
            (short)(elevationDelta * 100f),
            (short)timeDelta
        );
    }

    public static ScaledGain Create(short distanceDelta, short elevationDelta, short timeDelta) {
        return new ScaledGain(distanceDelta, elevationDelta, timeDelta);
    }
}

//scale 100x eg el gain of 0.01 -> 1
public readonly struct ScaledGain(short distanceDelta, short elevationDelta, short timeDelta) {
    public readonly short rawDistanceDelta = distanceDelta;
    public readonly short rawElevationDelta = elevationDelta;
    public readonly short rawTimeDelta = timeDelta;

    public float DistanceDelta => rawDistanceDelta / 100f;
    public float ElevationDelta => rawElevationDelta / 100f;
    public float TimeDelta => rawTimeDelta;
}

public record AnalyticData(List<GpxPoint> Data, List<GpxGain>? Gains = null);
