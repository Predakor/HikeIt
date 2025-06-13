using Domain.Common;
using Domain.Interfaces;

namespace Domain.ReachedPeaks;
public interface IReachedPeakRepository : ICrudRepository<ReachedPeak, Guid> {
    public Task<Result<List<ReachedPeak>>> GetReached();
    public Task<Result<List<ReachedPeak>>> AddRangeAsync(List<ReachedPeak> peaks);

}

