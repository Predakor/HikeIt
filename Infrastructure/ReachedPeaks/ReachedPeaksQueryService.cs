using Application.ReachedPeaks;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.ReachedPeaks;

internal class ReachedPeaksQueryService : IReachedPeaksQureryService {
    readonly TripDbContext _tripDbContext;

    public ReachedPeaksQueryService(TripDbContext tripDbContext) {
        _tripDbContext = tripDbContext;
    }

    public async Task<List<int>> ReachedByUserBefore(Guid userId, IEnumerable<int> peakIds) {
        if (peakIds.IsNullOrEmpty()) {
            return [];
        }

        return await _tripDbContext
            .ReachedPeaks.AsNoTracking()
            .Where(rp => rp.UserId == userId && peakIds.Contains(rp.PeakId))
            .Select(rp => rp.PeakId)
            .Distinct()
            .ToListAsync();
    }
}
