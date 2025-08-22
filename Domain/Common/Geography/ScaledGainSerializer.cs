using Domain.Common.Geography.ValueObjects;

namespace Domain.Common.Geography;

public static class ScaledGainSerializer {
    // 4 shorts per item (2 bytes each)
    readonly static short Size = 8;

    public static byte[] Serialize(GpxGain[] gains, float scale = 100) {
        var buffer = new byte[gains.Length * Size];
        for (int i = 0; i < gains.Length; i++) {
            var offset = i * Size;
            var scaled = ScaledGainFactory.FromGain(gains[i], scale);

            BitConverter.TryWriteBytes(buffer.AsSpan(offset, 2), scaled.ScaledDistanceDelta);
            BitConverter.TryWriteBytes(buffer.AsSpan(offset + 2, 2), scaled.ScaledElevationDelta);
            BitConverter.TryWriteBytes(buffer.AsSpan(offset + 4, 2), scaled.ScaledTimeDelta);
            BitConverter.TryWriteBytes(buffer.AsSpan(offset + 6, 2), scaled.Scale);
        }
        return buffer;
    }

    public static ScaledGain[] Deserialize(byte[] data) {
        var count = data.Length / Size;
        var gains = new ScaledGain[count];
        for (int i = 0; i < count; i++) {
            var offset = i * Size;
            short distance = BitConverter.ToInt16(data, offset);
            short elevation = BitConverter.ToInt16(data, offset + 2);
            short time = BitConverter.ToInt16(data, offset + 4);
            short scale = BitConverter.ToInt16(data, offset + 6);

            //keep values from scaling again
            gains[i] = ScaledGainFactory.FromRaw(distance, elevation, time, scale);
        }
        return gains;
    }
}
