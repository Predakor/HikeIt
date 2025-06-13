using Domain.Common;
using Domain.Trips.ValueObjects;

namespace Domain.Tests;

public class ScaledGainSerializerTests {
    [Fact]
    public void SerializeAndDeserialize_ShouldPreserveScaledValues() {
        // Arrange
        var originalGains = new[]
        {
            new GpxGain(10.5f, 2.3f, 1.1f),
            new GpxGain(5.2f, -1.7f, 0.8f),
            new GpxGain(3.0f, 0.0f, 0.0f),
            new GpxGain(.01f, 0.01f, 0.01f),
            new GpxGain(.01f, 0.01f, 0.001f),
            new GpxGain(0f, 0.0f, 0.0f),
        };
        float scale = 100;

        // Act
        byte[] serialized = ScaledGainSerializer.Serialize(originalGains, scale);
        var deserialized = ScaledGainSerializer.Deserialize(serialized);

        // Assert
        Assert.Equal(originalGains.Length, deserialized.Length);

        for (int i = 0; i < originalGains.Length; i++) {
            var expected = ScaledGainFactory.FromGain(originalGains[i], scale);
            var actual = deserialized[i];

            Assert.Equal(expected.ScaledDistanceDelta, actual.ScaledDistanceDelta);
            Assert.Equal(expected.ScaledElevationDelta, actual.ScaledElevationDelta);
            Assert.Equal(expected.ScaledTimeDelta, actual.ScaledTimeDelta);
            Assert.Equal(expected.Scale, actual.Scale);
        }
    }

    [Fact]
    public void SerializedGain_ShouldBeTheSame_AFterDeserialization() {
        var origianalGain = new GpxGain[] { new(10.5f, 2.3f, 1.1f) };

        var encodedGain = ScaledGainSerializer.Serialize(origianalGain);
        var decodedGain = ScaledGainSerializer.Deserialize(encodedGain)[0];

        var unscaledGain = new GpxGain(
            decodedGain.DistanceDelta,
            decodedGain.ElevationDelta,
            0,
            decodedGain.TimeDelta
        );

        var orgin = origianalGain[0];
        Assert.Equal(orgin.DistanceDelta, unscaledGain.DistanceDelta);
        Assert.Equal(orgin.ElevationDelta, unscaledGain.ElevationDelta);
        Assert.Equal(orgin.TimeDelta, unscaledGain.TimeDelta);
    }

    [Fact]
    public void Serialize_ShouldReturnExpectedByteLength() {
        // Arrange
        var gains = new[] { new GpxGain(1f, 1f, 1f), new GpxGain(2f, 2f, 2f) };

        // Act
        byte[] bytes = ScaledGainSerializer.Serialize(gains);

        // Assert
        Assert.Equal(16, bytes.Length); // 2 items * 8 bytes per item
    }

    [Fact]
    public void Deserialize_WithNonMultipleOfSize_ShouldIgnoreTrailingBytes() {
        // Arrange
        var badBytes = new byte[7]; // Not a multiple of 8

        // Act
        var result = ScaledGainSerializer.Deserialize(badBytes);

        // Assert
        Assert.Empty(result); // Should return 0 valid entries
    }
}
