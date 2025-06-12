using Domain.Trips.ValueObjects;

namespace Domain.Common;

public static class ScaledGainSerializer {
    public static byte[] Serialize(ScaledGain[] gains) {
        var buffer = new byte[gains.Length * 6]; // 3 shorts per item (2 bytes each)
        for (int i = 0; i < gains.Length; i++) {
            var offset = i * 6;
            BitConverter.TryWriteBytes(buffer.AsSpan(offset, 2), gains[i].rawDistanceDelta);
            BitConverter.TryWriteBytes(buffer.AsSpan(offset + 2, 2), gains[i].rawElevationDelta);
            BitConverter.TryWriteBytes(buffer.AsSpan(offset + 4, 2), gains[i].rawTimeDelta);
        }
        return buffer;
    }

    public static ScaledGain[] Deserialize(byte[] data) {
        var count = data.Length / 6;
        var gains = new ScaledGain[count];
        for (int i = 0; i < count; i++) {
            var offset = i * 6;
            short distance = BitConverter.ToInt16(data, offset);
            short elevation = BitConverter.ToInt16(data, offset + 2);
            short time = BitConverter.ToInt16(data, offset + 4);
            gains[i] = ScaledGainFactory.Create(distance, elevation, time);
        }
        return gains;
    }
}
