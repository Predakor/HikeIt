using Domain.Common;
using Domain.Interfaces;
using Domain.TripAnalytics.Entities.ElevationProfile;

namespace Domain.TripAnalytics.Repositories;
public interface IElevationProfileRepository : IRepository<ElevationProfile, Guid> {
    public Task<Result<ElevationProfile>> GetById(Guid id);
    public Task<Result<ElevationProfile>> Create(ElevationProfile profile);

}
