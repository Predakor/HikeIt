using Application.Commons.Interfaces;
using Domain.Common.Result;
using Domain.Peaks;
using Domain.ReachedPeaks;

namespace Application.ReachedPeaks;

public interface IReachedPeaksQureryService : IQueryService {
    Task<List<Peak>> ReachedByUserBefore(Guid userId, IEnumerable<int> peakIds);
    Task<Result<List<ReachedPeak>>> ReachedOnTrip(Guid tripId);
}
