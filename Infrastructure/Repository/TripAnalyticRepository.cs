using Domain.TripAnalytics;
using Domain.TripAnalytics.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;

namespace Infrastructure.Repository;

public class TripAnalyticRepository : Repository<TripAnalytic, Guid>, ITripAnalyticRepository {
    public TripAnalyticRepository(TripDbContext context)
        : base(context) { }

    public async Task<bool> AddAsync(TripAnalytic entity) {
        return await DbSet.AddAsync(entity) != null;

    }

    public Task<bool> RemoveAsync(Guid id) {
        throw new NotImplementedException();
    }
}
