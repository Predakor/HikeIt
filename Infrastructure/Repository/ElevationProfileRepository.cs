using Domain.Common;
using Domain.Common.Result;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Repositories;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;

namespace Infrastructure.Repository;

public class ElevationProfileRepository
    : Repository<ElevationProfile, Guid>,
        IElevationProfileRepository {
    public ElevationProfileRepository(TripDbContext context)
        : base(context) { }

    public async Task<Result<ElevationProfile>> GetById(Guid id) {
        var r = await DbSet.FindAsync(id);
        if (r == null) {
            return Errors.NotFound("no elevation profile for " + id + " found");
        }
        return r;
    }

    public async Task<Result<ElevationProfile>> AddAsync(ElevationProfile profile) {
        var ent = await DbSet.AddAsync(profile);
        return ent != null
            ? profile
            : Errors.Unknown("something went wrong while adding your profile to db");
    }
}
