using Domain.Common.Abstractions;

namespace Domain.ReachedPeaks;

public interface IReachedPeakRepository : ICrudRepository<ReachedPeak, Guid>
{
    Task<Result<IList<ReachedPeak>>> GetReached();
    Task<Result<IList<ReachedPeak>>> AddRangeAsync(IEnumerable<ReachedPeak> peaks);
}
