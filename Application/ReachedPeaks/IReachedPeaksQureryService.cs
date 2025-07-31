using Application.Interfaces;
using Domain.Mountains.Peaks;

namespace Application.ReachedPeaks;

public interface IReachedPeaksQureryService : IQueryService {
    Task<List<Peak>> ReachedByUserBefore(Guid userId, IEnumerable<int> peakIds);
}
