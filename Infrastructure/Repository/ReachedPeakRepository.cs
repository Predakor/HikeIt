using Domain.Common;
using Domain.ReachedPeaks;
using Infrastructure.Data;

namespace Infrastructure.Repository;

public class ReachedPeakRepository : Repository<ReachedPeak, Guid>, IReachedPeakRepository {
    public ReachedPeakRepository(TripDbContext context)
        : base(context) { }

    public async Task<bool> AddAsync(ReachedPeak entity) {
        var querry = await DbSet.AddAsync(entity);

        if (querry == null) {
            return false;
        }

        return await SaveChangesAsync();
    }

    public Task<Result<List<ReachedPeak>>> GetReached() {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(Guid id) {
        throw new NotImplementedException();
    }
}
