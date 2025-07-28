using Domain.Common.Utils;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Entities.PeaksAnalytics;

namespace Application.Dto.Analytics;

public record ReachedPeakDto(string Name, int Height, DateTime? ReachedAt = null);

public record PeakAnalyticsDto(List<ReachedPeakDto> Reached);

public static class ReachedPeaksExtentios {
    public static ReachedPeakDto ToDto(this ReachedPeak reachedPeak) {
        return new(reachedPeak.Peak.Name, reachedPeak.Peak.Height, reachedPeak.TimeReached);
    }

    public static PeakAnalyticsDto? ToDto(this PeaksAnalytic analytics) {
        if (analytics == null) {
            return null;
        }

        var reachedPeaks = analytics.ReachedPeaks.NotNullOrEmpty()
            ? analytics.ReachedPeaks?.Select(reachedPeak => reachedPeak.ToDto()).ToList()
            : [];

        return new(reachedPeaks);
    }
}
