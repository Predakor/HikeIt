using Domain.Common;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class TripAnalyticRepository : CrudRepository<TripAnalytic, Guid>, ITripAnalyticRepository {
    public TripAnalyticRepository(TripDbContext context)
        : base(context) { }

    public async Task<Result<TripAnalytic>> GetCompleteAnalytic(Guid id) {
        var analytics = await DbSet
            .Include(a => a.PeaksAnalytic)
            .Include(a => a.ElevationProfile)
            .FirstAsync(a => a.Id == id);

        if (analytics == null) {
            return Result<TripAnalytic>.Failure(Errors.NotFound("analytics"));
        }

        return Result<TripAnalytic>.Success(analytics);
    }
}
