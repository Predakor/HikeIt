using Domain.Common;
using Domain.Trips.ValueObjects;

namespace Domain.Tests;

public class ScaledGainTests {
    // Sample GpxGain input data
    public static IEnumerable<object[]> GainData => data.Select(g => new object[] { g });

    public static readonly GpxGain[] data =
    [
        new GpxGain(1.23f, 0.45f, 10f),
        new GpxGain(0f, 0f, 0f),
        new GpxGain(-0.5f, 0.99f, 5f),
        new GpxGain(-0.05f, 0.099f, 0.5f),
    ];

    [Theory]
    [MemberData(nameof(GainData))]
    public void ScaledGain_Scaling_RoundTrip(GpxGain gain) {
        var scaled = gain.ToScaled();
        float tolerance = 0.01f;

        Assert.InRange(
            scaled.DistanceDelta,
            gain.DistanceDelta - tolerance,
            gain.DistanceDelta + tolerance
        );
        Assert.InRange(
            scaled.ElevationDelta,
            gain.ElevationDelta - tolerance,
            gain.ElevationDelta + tolerance
        );
        Assert.Equal(gain.TimeDelta ?? 0, scaled.TimeDelta);
    }

    [Fact]
    public void ScaledGainSerializer_Should_SerializeAndDeserialize() {
        var gains = data.Select(g => g.ToScaled()).ToArray();

        var bytes = ScaledGainSerializer.Serialize(gains);
        var deserialized = ScaledGainSerializer.Deserialize(bytes);

        Assert.Equal(gains.Length, deserialized.Length);

        for (int i = 0; i < gains.Length; i++) {
            Assert.InRange(
                deserialized[i].DistanceDelta,
                gains[i].DistanceDelta - 0.01f,
                gains[i].DistanceDelta + 0.01f
            );
            Assert.InRange(
                deserialized[i].ElevationDelta,
                gains[i].ElevationDelta - 0.01f,
                gains[i].ElevationDelta + 0.01f
            );
            Assert.Equal((short)gains[i].TimeDelta, (short)deserialized[i].TimeDelta);
        }
    }

    [Fact]
    public void ScaledGain_ShouldHave_DefaultTime() {
        GpxGain gain = new(1, 1, 1);
        var scaledGain = gain.ToScaled();

        Assert.Equal(default, scaledGain.TimeDelta);
    }

    [Fact]
    public void DeserializedSerializedValue_ShouldBe_TheSameAS_Original() {
        GpxGain gain = new(1, 1, 1);
        var original = gain.ToScaled();

        var bytes = ScaledGainSerializer.Serialize([original]);
        var serialized = ScaledGainSerializer.Deserialize(bytes);

        Console.WriteLine(bytes);

        Assert.Equal(serialized[0].DistanceDelta, original.DistanceDelta);
        Assert.Equal(serialized[0].ElevationDelta, original.ElevationDelta);
        Assert.Equal(serialized[0].TimeDelta, original.TimeDelta);
    }

}
