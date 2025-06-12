using Domain.Common;
using Domain.TripAnalytics.Entities.ElevationProfile;

namespace Application.TripAnalytics.Services;
public interface IElevationProfileService {
    public Task<Result<ElevationProfile>> FindOrCreate(ElevationProfile profile);

}
