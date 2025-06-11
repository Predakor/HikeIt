using Domain.Common;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Services;

public class TripDomainAnalyticsService : ITripDomainAnalyticService {
    public List<GpxPoint> FindLocalPeaks(List<GpxPoint> points, List<GpxGain> gains) {
        var localPeaks = new List<GpxPoint>();

        for (int i = 1; i < gains.Count; i++) {
            var current = gains[i];
            var prev = gains[i - 1];

            bool isDescending = current.ElevationDelta < prev.ElevationDelta;
            if (isDescending) {
                localPeaks.Add(points[i]);
            }
        }

        return localPeaks;
    }

    public List<GpxGain> GenerateGains(List<GpxPoint> points) {
        return points.ToGains();
    }
}
