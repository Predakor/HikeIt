using Application.ReachedPeaks;
using Domain.Mountains.Peaks;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.ReachedPeaks;

public class ReachedPeaksQueryService : IReachedPeaksQureryService {
    readonly TripDbContext _tripDbContext;

    public ReachedPeaksQueryService(TripDbContext tripDbContext) {
        _tripDbContext = tripDbContext;
    }

    public async Task<List<Peak>> ReachedByUserBefore(Guid userId, IEnumerable<int> peakIds) {
        if (peakIds.IsNullOrEmpty()) {
            return [];
        }

        return await _tripDbContext
            .ReachedPeaks.AsNoTracking()
            .Include(rp => rp.Peak)
            .Where(rp => rp.UserId == userId && peakIds.Contains(rp.PeakId))
            .Select(rp => rp.Peak)
            .Distinct()
            .ToListAsync();
    }
}
