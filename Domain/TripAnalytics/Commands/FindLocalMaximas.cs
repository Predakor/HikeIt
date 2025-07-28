using Domain.Common;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Commands;

public static class AnalyticDataExtentions {
    public static List<GpxPoint> ToLocalMaxima(this IEnumerable<GpxPoint> gpxPoints) {
        return FindLocalMaxima([.. gpxPoints]);
    }

    public static List<GpxPoint> ToLocalMaxima(this AnalyticData data) {
        return FindLocalMaxima(data.Points, data.Gains);
    }

    static List<GpxPoint> FindLocalMaxima(List<GpxPoint> points, List<GpxGain>? gains = null) {
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
}
