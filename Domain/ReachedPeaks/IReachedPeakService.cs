using Domain.Common.Geography.ValueObjects;
using Domain.Locations.Peaks;
using Domain.Trips.Root;
using Domain.Users.Root;

namespace Domain.ReachedPeaks;

public interface IReachedPeakService {
    Task<Result<List<ReachedPeak>>> CreateReachedPeaks(AnalyticData data, Trip trip);

    Result<ReachedPeak> ToReachedPeak(Peak peak, Trip trip, User user);
}
