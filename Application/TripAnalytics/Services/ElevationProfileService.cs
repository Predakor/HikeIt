using Domain.Common;
using Domain.Common.Result;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Repositories;
using Domain.TripAnalytics.Services;

namespace Application.TripAnalytics.Services;

public class ElevationProfileService : IElevationProfileService {
    readonly IElevationProfileRepository _repository;

    public ElevationProfileService(IElevationProfileRepository repository) {
        _repository = repository;
    }

    public async Task<Result<ElevationProfile>> Create(ElevationProfile profile) {
        var query = await _repository.AddAsync(profile);
        return query;
    }

    public async Task<Result<ElevationProfile>> FindOrCreate(ElevationProfile profile) {
        return await _repository
            .GetById(profile.Id)
            .MatchAsync(
                profile => profile,
                async error => error is Error.NotFound ? await Create(profile) : error
            );
    }

    public Task<Result<ElevationProfile>> GetById(Guid id) {
        throw new NotImplementedException();
    }
}
