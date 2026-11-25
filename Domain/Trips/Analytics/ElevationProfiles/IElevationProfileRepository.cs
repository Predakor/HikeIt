using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.ElevationProfiles;

public interface IElevationProfileRepository : IRepository<ElevationProfile, Guid>
{
    Task<Result<ElevationProfile>> GetById(Guid id);
    Task<Result<ElevationProfile>> AddAsync(ElevationProfile profile);
}
