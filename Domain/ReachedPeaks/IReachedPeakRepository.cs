using Domain.Common.Abstractions;
using Domain.Common.Result;

namespace Domain.ReachedPeaks;
public interface IReachedPeakRepository : ICrudRepository<ReachedPeak, Guid> {
    public Task<Result<IList<ReachedPeak>>> GetReached();
    public Task<Result<IList<ReachedPeak>>> AddRangeAsync(IEnumerable<ReachedPeak> peaks);

}

