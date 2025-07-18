using Domain.Common;
using Domain.Common.Result;
using Domain.Entiites.Peaks;
using Domain.Entiites.Regions;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;


public class RegionRepository : ResultRepository<Region, int>, IRegionRepository {
    public RegionRepository(TripDbContext context) : base(context) { }

    public async Task<Result<List<Peak>>> AllPeaksFromRegion(Region region) {
        var peaks = await Context.Peaks.Where(peak => region.Id == peak.RegionID).ToListAsync();

        if (peaks.Count == 0) {
            return Errors.NotFound($"no peaks found in region: {region.Name}");
        }
        return peaks;

    }
}
