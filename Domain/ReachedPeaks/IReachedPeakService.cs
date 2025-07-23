using Domain.Common.Result;
using Domain.Mountains.Peaks;
using Domain.Trips;
using Domain.Users;

namespace Domain.ReachedPeaks;

public interface IReachedPeakService {
    Result<ReachedPeak> ToReachedPeak(Peak peak, Trip trip, User user);
    Result<IList<ReachedPeak>> ToReachedPeaks(IEnumerable<Peak> peaks, Guid tripId, Guid userId);
}
