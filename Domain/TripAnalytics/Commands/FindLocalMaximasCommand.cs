using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Common.Result;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Commands;

public class FindLocalMaximasCommand(AnalyticData data) : ICommand<List<GpxPoint>> {
    public Result<List<GpxPoint>> Execute() {
        var (points, gains) = data;
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

    public static ICommand<List<GpxPoint>> Create(AnalyticData data) {
        return new FindLocalMaximasCommand(data);
    }
}
