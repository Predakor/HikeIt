using Domain.Trips.ValueObjects;

namespace Domain.Tests;
public class ScaledGainTests {

    [Theory]
    [InlineData(1.23f, 0.45f, 10f)]
    [InlineData(0f, 0f, 0f)]
    [InlineData(-0.5f, 0.99f, 5f)]
    [InlineData(-0.05f, 0.099f, 0.5f)]

    public void ScaledGain_Scaling_RoundTrip(float distanceDelta, float elevationDelta, float timeDelta) {
        var gain = new ScaledGain(distanceDelta, elevationDelta, timeDelta);

        Assert.InRange(gain.DistanceDelta, distanceDelta - 0.01f, distanceDelta + 0.01f);
        Assert.InRange(gain.ElevationDelta, elevationDelta - 0.01f, elevationDelta + 0.01f);
        Assert.Equal((short)timeDelta, (short)gain.TimeDelta);
    }
}