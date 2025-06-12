namespace Domain.Trips.ValueObjects;

public interface IGeoGain {
    float DistanceDelta { get; }
    float ElevationDelta { get; }
    float Slope { get; }
}

public record GpxGain(
    float DistanceDelta,
    float ElevationDelta,
    float Slope,
    float? TimeDelta = 0
) : IGeoGain;

public record GpxGainWithTime(
    float DistanceDelta,
    float ElevationDelta,
    float Slope,
    float TimeDelta
) : IGeoGain;
