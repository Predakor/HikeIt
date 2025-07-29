using Domain.Common;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Commands;

public static class AnalyticDataExtentions {
    public static List<GpxPoint> ToLocalMaxima(this AnalyticData data) {
        return LocalMaximaIndicesWithDistance(data.Points, data.Gains)
            .Select(result => data.Points[result.index])
            .ToList();
    }

    public static List<GpxPointWithDistance> ToLocalMaximaWithDistance(this AnalyticData data) {
        return LocalMaximaIndicesWithDistance(data.Points, data.Gains)
            .Select(result => new GpxPointWithDistance(
                data.Points[result.index],
                (float)result.distanceFromStart
            ))
            .ToList();
    }

    public static List<T> WithProximityMerge<T>(this List<T> points)
        where T : IGeoPoint {
        return points.MergeNearbyPeakByDistance().MergeNearbyPeaksByElevation();
    }

    internal static List<T> MergeNearbyPeaksByElevation<T>(this List<T> peaks, int minDistance = 50)
        where T : IGeoPoint {
        bool minElevationTreshold(T curr, T prev) {
            var eleDelta = Math.Abs(curr.Ele - prev.Ele);
            return eleDelta > minDistance;
        }

        return peaks.MergePeaksBy(minElevationTreshold);
    }

    internal static List<T> MergeNearbyPeakByDistance<T>(this List<T> peaks, int minDistance = 200)
        where T : IGeoPoint {
        bool minDistanceDelta(T curr, T prev) {
            var distDelta = Math.Abs(DistanceHelpers.Distance3D(curr, prev));
            return distDelta > minDistance;
        }

        return peaks.MergePeaksBy(minDistanceDelta);
    }

    internal static IEnumerable<(int index, double distanceFromStart)> LocalMaximaIndicesWithDistance(
        List<GpxPoint> points,
        List<GpxGain>? gains = null
    ) {
        gains ??= points.ToGains();

        bool isAscendingFlag = true;
        double distanceFromStart = 0;

        for (int i = 1; i < gains.Count; i++) {
            var current = gains[i];
            distanceFromStart += current.DistanceDelta;

            bool isDescending = current.ElevationDelta < 0;

            if (!isDescending) {
                isAscendingFlag = true;
            }

            if (isDescending && isAscendingFlag) {
                isAscendingFlag = false;
                yield return (i, distanceFromStart);
            }
        }
    }

    internal static List<T> MergePeaksBy<T>(this List<T> peaks, Func<T, T, bool> predicate)
        where T : IGeoPoint {
        int itemCount = peaks.Count;
        if (itemCount < 2) {
            return peaks;
        }

        var mergedPoints = new List<T>();

        var windowStart = 0;

        for (int i = 1; i < itemCount; i++) {
            var current = peaks[i];
            var prev = peaks[i - 1];

            if (!predicate(current, prev)) {
                continue;
            }

            var windowSize = i - windowStart;
            var windowCenter = windowStart + (windowSize / 2);

            mergedPoints.Add(peaks[windowCenter]);

            windowStart = i + 1;
        }

        //handle last window not closing
        if (windowStart < itemCount) {
            int windowSize = itemCount - 1 - windowStart;
            int centerIndex = windowStart + (windowSize / 2);
            mergedPoints.Add(peaks[centerIndex]);
        }

        if (mergedPoints.Count == 0) {
            return peaks;
        }

        return mergedPoints;
    }
}
