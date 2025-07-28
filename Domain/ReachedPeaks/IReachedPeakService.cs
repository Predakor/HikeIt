using Domain.Common.Result;
using Domain.Mountains.Peaks;
using Domain.Trips;
using Domain.Trips.ValueObjects;
using Domain.Users;

namespace Domain.ReachedPeaks;

public interface IReachedPeakService {
    Task<Result<List<ReachedPeak>>> GetPeaks(
        AnalyticData data,
        Guid tripId,
        Guid userId
    );

    Result<ReachedPeak> ToReachedPeak(Peak peak, Trip trip, User user);
    Result<List<ReachedPeak>> ToReachedPeaks(IEnumerable<Peak> peaks, Guid tripId, Guid userId);
}
