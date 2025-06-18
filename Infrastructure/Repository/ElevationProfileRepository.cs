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
    public ElevationProfileRepository(TripDbContext context) : base(context) {
    }

    public async Task<Result<ElevationProfile>> GetById(Guid id) {
        var r = await DbSet.FindAsync(id);
        if (r == null) {
            Error err = Errors.NotFound("no elevation profile for " + id + " found");
            return RepoResult.Fail<ElevationProfile>(err);
        }
        return RepoResult.Success(r);
    }

    public async Task<Result<ElevationProfile>> Create(ElevationProfile profile) {
        var result = await DbSet.AddAsync(profile);

        if (result == null) {
            Error error = Errors.Unknown("something went wrong");
            return RepoResult.Fail<ElevationProfile>(error);
        }

        return RepoResult.Success(profile);
    }


    static class RepoResult {
        public static Result<T> Success<T>(T id) => Result<T>.Success(id);

        public static Result<T> Fail<T>(Error error) => Result<T>.Failure(error);
    }
}
