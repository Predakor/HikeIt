using Domain.Common.Utils;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Entities.PeaksAnalytics;

namespace Application.Dto.Analytics;

public record ReachedPeakDto(string Name, int Height, DateTime? ReachedAt = null);

public record PeakAnalyticsDto(uint Total, uint Unique, uint New, List<ReachedPeakDto> Reached);

public static class ReachedPeaksExtentios {
    public static ReachedPeakDto ToDto(this ReachedPeak reachedPeak) {
        return new(reachedPeak.Peak.Name, reachedPeak.Peak.Height, reachedPeak.ReachedAtTime);
    }

    public static PeakAnalyticsDto ToDto(this PeaksAnalytic analytics, IEnumerable<ReachedPeak> peaks) {
        var reachedPeaks = peaks.NotNullOrEmpty()
            ? peaks.Select(reachedPeak => reachedPeak.ToDto()).ToList()
            : [];

        return new(analytics.Total, analytics.Unique, analytics.New, reachedPeaks);
    }
}
