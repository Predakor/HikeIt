namespace Domain.Common.SlidingWindow;

public static class DynamicSlidingWindow {
    public static IEnumerable<List<T>> DynamicWindow<T>(
        this IReadOnlyList<T> points,
        Func<T, T, bool> shouldSplit
    ) {
        if (points.Count == 0) {
            yield break;
        }

        var window = new List<T> { points[0] };

        for (int i = 1; i < points.Count; i++) {
            var current = points[i];
            var previous = points[i - 1];

            if (shouldSplit(current, previous)) {
                yield return new List<T>(window);
                window.Clear();
            }

            window.Add(current);
        }

        if (window.Count > 0) {
            yield return window;
        }
    }
}

public static class DynamicSlidingWindowExtentions {
    public static List<TOut> GroupByDynamicWindow<T, TOut>(
        this IReadOnlyList<T> points,
        Func<T, T, bool> shouldSplit,
        Func<List<T>, TOut> aggregate
    ) {
        return points.DynamicWindow(shouldSplit).Select(window => aggregate(window)).ToList();
    }
}
