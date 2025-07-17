using Domain.Common;
using Domain.Common.Result;
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
            .Include(a => a.PeaksAnalytic)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (analytics == null) {
            return Errors.NotFound("analytics");
        }

        if (analytics.PeaksAnalytic != null) {
            var reachedPeaks = Context
                .ReachedPeaks.Where(peak => peak.TripId == id)
                .Include(p => p.Peak);
            analytics.PeaksAnalytic.ReachedPeaks = reachedPeaks.ToList();
        }

        return analytics;
    }
}
