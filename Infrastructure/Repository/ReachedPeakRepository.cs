using Domain.Common;
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

    public async Task<Result<List<ReachedPeak>>> AddRangeAsync(List<ReachedPeak> peaks) {
        try {
            await DbSet.AddRangeAsync(peaks);
            return Result<List<ReachedPeak>>.Success(peaks);
        }
        catch (Exception err) {
            return Result<List<ReachedPeak>>.Failure(Errors.Unknown(err.Message));
            throw;
        }
    }

    public Task<Result<List<ReachedPeak>>> GetReached() {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(Guid id) {
        throw new NotImplementedException();
    }
}
