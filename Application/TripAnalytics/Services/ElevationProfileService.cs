using Domain.Common;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Repositories;

namespace Application.TripAnalytics.Services;

public class ElevationProfileService : IElevationProfileService {
    readonly IElevationProfileRepository _repository;

    public ElevationProfileService(IElevationProfileRepository repository) {
        _repository = repository;
    }

    public async Task<Result<ElevationProfile>> Create(ElevationProfile profile) {
        var query = await _repository.Create(profile);
        return query;
    }

    public async Task<Result<ElevationProfile>> FindOrCreate(ElevationProfile profile) {
        var query = await _repository.GetById(profile.Id);

        var con = query.AsyncMap(
            async found => {
                await Task.CompletedTask;
                return Result<ElevationProfile>.Success(found);
            },
            async notFound => {
                var res = await Create(profile);
                return res;
            },
            async error => {
                await Task.CompletedTask;
                return Result<ElevationProfile>.Failure(error);
            }
        );
        return await con;
    }

    public Task<Result<ElevationProfile>> GetById(Guid id) {
        throw new NotImplementedException();
    }
}
