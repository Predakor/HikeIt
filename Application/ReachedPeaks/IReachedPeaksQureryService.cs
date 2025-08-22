using Application.Commons.Abstractions.Queries;
using Domain.Locations.Peaks;
using Domain.ReachedPeaks;

namespace Application.ReachedPeaks;

public interface IReachedPeaksQureryService : IQueryService {
    Task<List<Peak>> ReachedByUserBefore(Guid userId, IEnumerable<int> peakIds);
    Task<Result<List<ReachedPeak>>> ReachedOnTrip(Guid tripId);
}
