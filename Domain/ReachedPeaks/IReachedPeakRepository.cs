using Domain.Common;

namespace Domain.ReachedPeaks;
public interface IReachedPeakRepository : ICrudRepository<ReachedPeak, Guid> {
    public Task<Result<List<ReachedPeak>>> GetReached();
}

