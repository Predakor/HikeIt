using Application.ReachedPeaks;
using Domain.Common;
using Domain.Common.Extentions;
using Domain.Common.Result;
using Domain.Locations.Peaks;
using Domain.ReachedPeaks;
using Infrastructure.Commons.Databases;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ReachedPeaks.Queries;

public class ReachedPeaksQueryService : IReachedPeaksQureryService {
    readonly TripDbContext _tripDbContext;

    IQueryable<ReachedPeak> ReachedPeaks => _tripDbContext.ReachedPeaks.AsNoTracking();

    public ReachedPeaksQueryService(TripDbContext tripDbContext) {
        _tripDbContext = tripDbContext;
    }

    public async Task<List<Peak>> ReachedByUserBefore(Guid userId, IEnumerable<int> peakIds) {
        if (peakIds.NullOrEmpty()) {
            return [];
        }

        return await ReachedPeaks
            .Include(rp => rp.Peak)
            .Where(rp => rp.UserId == userId && peakIds.Contains(rp.PeakId))
            .Select(rp => rp.Peak)
            .Distinct()
            .ToListAsync();
    }

    public async Task<Result<List<ReachedPeak>>> ReachedOnTrip(Guid tripId) {
        var query = await ReachedPeaks
            .Include(rp => rp.Peak)
            .Where(rp => rp.TripId == tripId)
            .ToListAsync();

        if (query.NullOrEmpty()) {
            return Errors.EmptyCollection("reached peaks");
        }

        return query;
    }
}
