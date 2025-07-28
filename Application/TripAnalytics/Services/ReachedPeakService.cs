using Domain.Common;
using Domain.Common.Result;
using Domain.Mountains.Peaks;
using Domain.ReachedPeaks;
using Domain.Trips;
using Domain.Users;

namespace Application.TripAnalytics.Services;

public class ReachedPeakService : IReachedPeakService {
    public Result<ReachedPeak> ToReachedPeak(Peak peak, Trip trip, User user) {
        return ReachedPeak.Create(peak, trip, user);
    }

    public Result<IList<ReachedPeak>> ToReachedPeaks(IEnumerable<Peak> peaks, Trip trip, User user) {
        if (!peaks.Any()) {
            return Errors.EmptyCollection("Peaks");
        }

        Console.WriteLine($"Matched {peaks.Count()} Peaks");

        return peaks.Select(p => ReachedPeak.Create(p, trip, user)).ToList();
    }
}
