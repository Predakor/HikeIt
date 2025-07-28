using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Common.Result;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Entities.PeaksAnalytics;

namespace Application.TripAnalytics.Services;

public record PeakAnalyticsData(
    IEnumerable<ReachedPeak> Peaks,
    IEnumerable<ReachedPeak>? NewPeaks = null
);

public class CreatePeakAnalyticsCommand(Guid id, PeakAnalyticsData data) : ICommand<PeaksAnalytic> {
    public Result<PeaksAnalytic> Execute() {
        var (peaks, _) = data;

        if (!peaks.Any()) {
            return Errors.EmptyCollection("Reached Peaks");
        }

        var analytics = PeaksAnalytic.Create(
            id,
            [.. peaks]
        );

        if (analytics == null) {
            return Errors.Unknown("Failed To create Peak Analytics");
        }

        return analytics;
    }

    public static ICommand<PeaksAnalytic> Create(Guid id, PeakAnalyticsData data) {
        return new CreatePeakAnalyticsCommand(id, data);
    }
}
