using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Common.Result;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Entities.PeaksAnalytics;

namespace Application.TripAnalytics.Services;

public class CreatePeakAnalyticsCommand(Guid id, IEnumerable<ReachedPeak> peaks)
    : ICommand<PeaksAnalytic> {
    public Result<PeaksAnalytic> Execute() {
        if (!peaks.Any()) {
            return Errors.EmptyCollection("Reached Peaks");
        }

        var analytics = PeaksAnalytic.Create(id, [.. peaks]);

        if (analytics == null) {
            return Errors.Unknown("Failed To create Peak Analytics");
        }

        return analytics;
    }

    public static ICommand<PeaksAnalytic> Create(Guid id, IEnumerable<ReachedPeak> peaks) {
        return new CreatePeakAnalyticsCommand(id, peaks);
    }
}
