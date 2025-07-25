namespace Domain.Users.Extentions;

public enum UpdateMode {
    Increase,
    Decrease,
    Set,
}
internal static class SafeUpdateExtentions {
    public static uint SafeUpdate(
        this uint current,
        uint delta,
        UpdateMode mode = UpdateMode.Increase
    ) {
        return mode switch {
            UpdateMode.Increase => SafeIncrement(current, delta),
            UpdateMode.Decrease => SafeDecrement(current, delta),
            UpdateMode.Set => SafeSet(delta),
            _ => current,
        };
    }

    public static uint SafeIncrement(uint current, uint delta) => current + delta;

    public static uint SafeDecrement(uint current, uint delta) => Math.Max(current - delta, 0);

    public static uint SafeSet(uint delta) => Math.Max(delta, 0);

    public static TimeSpan SafeUpdate(this TimeSpan current, TimeSpan timeDelta, UpdateMode mode) {
        return mode switch {
            UpdateMode.Set => timeDelta,
            UpdateMode.Increase => current.Add(timeDelta),
            UpdateMode.Decrease => SafeDecrement(current, timeDelta),
            _ => default,
        };
    }

    public static TimeSpan SafeDecrement(TimeSpan current, TimeSpan delta) {
        var newDuration = current.Subtract(delta);
        var minDuration = TimeSpan.Zero;
        return newDuration > minDuration ? newDuration : minDuration;
    }
}
