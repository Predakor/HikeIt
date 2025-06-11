using Domain.TripAnalytics;
using Domain.TripAnalytics.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repository;

public class TripAnalyticRepository : Repository<TripAnalytic, Guid>, ITripAnalyticRepository {
    public TripAnalyticRepository(TripDbContext context)
        : base(context) { }

    public async Task<bool> AddAsync(TripAnalytic entity) {
        var r = await DbSet.AddAsync(entity);
        if (await SaveChangesAsync()) {
            return true;
        }
        return false;
    }

    public Task<bool> RemoveAsync(Guid id) {
        throw new NotImplementedException();
    }
}
