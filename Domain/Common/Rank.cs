namespace Domain.Common;

public readonly record struct Rank(ushort Position, ushort Size) {
    public Rank(int position, int size)
        : this(position.ToUshort(), size.ToUshort()) { }

    public override string ToString() {
        return $"{Position}/{Size}";
    }
}

public static partial class RankExtentions {
    public static ushort ToUshort(this int value) {
        return (ushort)Math.Clamp(value, 0, uint.MaxValue);
    }
}
