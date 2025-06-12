namespace Domain.Trips.ValueObjects;

public static class ScaledGainFactory {
    // From float input (user/code)
    public static ScaledGain FromFloats(
        float distance,
        float elevation,
        float time,
        float? scale = null
    ) {
        return new ScaledGain(distance, elevation, time, scale ?? 100);
    }

    public static ScaledGain FromGain(GpxGain gain, float? scale = null) {
        return FromFloats(gain.DistanceDelta, gain.ElevationDelta, gain.TimeDelta ?? 0, scale);
    }

    // For deserialization only
    internal static ScaledGain FromRaw(short distance, short elevation, short time, short scale) {
        return new ScaledGain(distance, elevation, time, scale);
    }
}

//scale 100x eg el gain of 0.01 -> 1
public readonly struct ScaledGain {
    public readonly short ScaledDistanceDelta;
    public readonly short ScaledElevationDelta;
    public readonly short ScaledTimeDelta;
    public readonly short Scale;

    public ScaledGain(float distDelta, float eleDelta, float timeDelta, float scale) {
        ScaledDistanceDelta = (short)(distDelta * scale);
        ScaledElevationDelta = (short)(eleDelta * scale);
        ScaledTimeDelta = (short)timeDelta;
        Scale = (short)scale;
    }

    public ScaledGain(short distDelta, short eleDelta, short timeDelta, short scale) {
        ScaledDistanceDelta = distDelta;
        ScaledElevationDelta = eleDelta;
        ScaledTimeDelta = timeDelta;
        Scale = scale;
    }

    public float DistanceDelta => ScaledDistanceDelta / (float)Scale;
    public float ElevationDelta => ScaledElevationDelta / (float)Scale;
    public float TimeDelta => ScaledTimeDelta;
}
