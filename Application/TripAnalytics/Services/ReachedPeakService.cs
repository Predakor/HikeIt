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

    public Result<IList<ReachedPeak>> ToReachedPeaks(IEnumerable<Peak> peaks, Guid tripId, Guid userId) {

        if (!peaks.Any()) {
            return Errors.EmptyCollection("Peaks");
        }
        Console.WriteLine(peaks.Count());

        foreach (var peak in peaks) {
            Console.WriteLine(peak.Name);
        }

        return peaks.Select(p => ReachedPeak.Create(p.Id, tripId, userId)).ToList();
    }
}
