using Domain.Common.Geography;
using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;
using Domain.Common.SlidingWindow;

namespace Domain.Trips.Analytics.ElevationProfiles.Commands;

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

        return peaks.GroupByDynamicWindow(minElevationTreshold, Select.Highest);
    }

    internal static List<T> MergeNearbyPeakByDistance<T>(this List<T> peaks, int minDistance = 50)
        where T : IGeoPoint {
        bool minDistanceDelta(T curr, T prev) {
            var distDelta = Math.Abs(DistanceHelpers.Distance3D(curr, prev));
            return distDelta > minDistance;
        }

        return peaks.GroupByDynamicWindow(minDistanceDelta, Select.Highest);
    }

    internal static IEnumerable<(
        int index,
        double distanceFromStart
    )> LocalMaximaIndicesWithDistance(List<GpxPoint> points, List<GpxGain>? gains) {
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
}

static class Select {
    public static T Highest<T>(List<T> groupedPoints)
        where T : IGeoPoint => groupedPoints.MaxBy(point => point.Ele)!;

    public static T Center<T>(List<T> groupedPoints)
        where T : IGeoPoint => groupedPoints.ElementAt((groupedPoints.Count - 1) / 2);

    public static T Median<T>(List<T> groupedPoints)
        where T : IGeoPoint => Center(groupedPoints.OrderBy(p => p.Ele).ToList());
}
