using Domain.Common;
using Domain.Common.Result;
using Domain.ReachedPeaks;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;

namespace Infrastructure.Repository;

public class ReachedPeakRepository : Repository<ReachedPeak, Guid>, IReachedPeakRepository {
    public ReachedPeakRepository(TripDbContext context)
        : base(context) { }

    public async Task<bool> AddAsync(ReachedPeak entity) {
        var querry = await DbSet.AddAsync(entity);
        return querry != null;
    }

    public async Task<Result<IList<ReachedPeak>>> AddRangeAsync(IEnumerable<ReachedPeak> peaks) {
        try {
            await DbSet.AddRangeAsync(peaks);
            return peaks.ToList();
        }
        catch (Exception err) {
            return Errors.Unknown(err.Message);
            throw;
        }
    }

    public Task<Result<IList<ReachedPeak>>> GetReached() {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(Guid id) {
        throw new NotImplementedException();
    }
}
