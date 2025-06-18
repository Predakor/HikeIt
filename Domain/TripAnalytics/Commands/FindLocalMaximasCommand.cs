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

        for (int i = 1; i < gains.Count; i++) {
            var current = gains[i];
            var prev = gains[i - 1];

            //its catching every point while descending
            bool isDescending = current.ElevationDelta < prev.ElevationDelta;
            if (isDescending) {
                localPeaks.Add(points[i]);
            }
        }

        return localPeaks;
    }

    public static ICommand<List<GpxPoint>> Create(AnalyticData data) {
        return new FindLocalMaximasCommand(data);
    }
}
