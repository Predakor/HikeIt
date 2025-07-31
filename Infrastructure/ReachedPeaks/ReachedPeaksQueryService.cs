using Application.ReachedPeaks;
using Domain.Common;
using Domain.Common.Result;
using Domain.Mountains.Peaks;
using Domain.ReachedPeaks;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.ReachedPeaks;

public class ReachedPeaksQueryService : IReachedPeaksQureryService {
    readonly TripDbContext _tripDbContext;

    IQueryable<ReachedPeak> ReachedPeaks => _tripDbContext.ReachedPeaks.AsNoTracking();

    public ReachedPeaksQueryService(TripDbContext tripDbContext) {
        _tripDbContext = tripDbContext;
    }

    public async Task<List<Peak>> ReachedByUserBefore(Guid userId, IEnumerable<int> peakIds) {
        if (peakIds.IsNullOrEmpty()) {
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

        if (query.IsNullOrEmpty()) {
            return Errors.EmptyCollection("reached peaks");
        }

        return query;
    }
}
