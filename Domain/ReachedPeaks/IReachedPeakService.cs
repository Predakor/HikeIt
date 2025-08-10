using Domain.Common.Result;
using Domain.Peaks;
using Domain.Trips;
using Domain.Trips.ValueObjects;
using Domain.Users;

namespace Domain.ReachedPeaks;

public interface IReachedPeakService {
    Task<Result<List<ReachedPeak>>> CreateReachedPeaks(AnalyticData data, Trip trip);

    Result<ReachedPeak> ToReachedPeak(Peak peak, Trip trip, User user);
}
