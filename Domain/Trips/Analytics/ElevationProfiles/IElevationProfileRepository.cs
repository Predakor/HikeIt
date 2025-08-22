using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.ElevationProfiles;

public interface IElevationProfileRepository : IRepository<ElevationProfile, Guid> {
    public Task<Result<ElevationProfile>> GetById(Guid id);
    public Task<Result<ElevationProfile>> AddAsync(ElevationProfile profile);
}
