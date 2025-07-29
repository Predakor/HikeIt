using Application.Interfaces;

namespace Application.ReachedPeaks;

public interface IReachedPeaksQureryService : IQueryService {
    Task<List<int>> ReachedByUserBefore(Guid userId, IEnumerable<int> peakIds);
}
