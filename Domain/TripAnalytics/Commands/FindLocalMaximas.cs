using Domain.Common;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Commands;

public static class AnalyticDataExtentions {
    public static List<GpxPoint> ToLocalMaxima(this AnalyticData data) {
        return FindLocalMaxima(data.Points, data.Gains);
    }

    public static List<GpxPoint> WithProximityMerge(this List<GpxPoint> points) {
        return points.MergeNearbyPeakByDistance().MergeNearbyPeaksByElevation();
    }

    internal static List<GpxPoint> FindLocalMaxima(
        List<GpxPoint> points,
        List<GpxGain>? gains = null
    ) {
        gains ??= points.ToGains();

        var localPeaks = new List<GpxPoint>();
        bool isAscendingFlag = true;

        for (int i = 1; i < gains.Count; i++) {
            var current = gains[i];

            bool isDescending = current.ElevationDelta < 0;

            //flip flag after we start ascending again
            if (!isDescending) {
                isAscendingFlag = true;
            }

            if (isDescending) {
                //dont collect every descending point
                if (!isAscendingFlag) {
                    continue;
                }

                isAscendingFlag = false;
                localPeaks.Add(points[i]);
            }
        }

        return localPeaks;
    }

    internal static List<GpxPoint> MergeNearbyPeaksByElevation(
        this List<GpxPoint> peaks,
        int minDistance = 50
    ) {
        bool minElevationTreshold(GpxPoint curr, GpxPoint prev) {
            var eleDelta = Math.Abs(curr.Ele - prev.Ele);
            return eleDelta > minDistance;
        }

        return peaks.MergePeaksBy(minElevationTreshold);
    }

    internal static List<GpxPoint> MergeNearbyPeakByDistance(
        this List<GpxPoint> peaks,
        int minDistance = 200
    ) {
        bool minDistanceDelta(GpxPoint curr, GpxPoint prev) {
            var distDelta = Math.Abs(DistanceHelpers.Distance3D(curr, prev));
            return distDelta > minDistance;
        }

        return peaks.MergePeaksBy(minDistanceDelta);
    }

    static List<GpxPoint> MergePeaksBy(
        this List<GpxPoint> peaks,
        Func<GpxPoint, GpxPoint, bool> predicate
    ) {
        int itemCount = peaks.Count;
        if (itemCount < 2) {
            return peaks;
        }

        var mergedPoints = new List<GpxPoint>();

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
