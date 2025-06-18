using Domain.Common.Result;
using Domain.TripAnalytics.Entities.ElevationProfile;

namespace Domain.TripAnalytics.Services;
public interface IElevationProfileService {
    public Task<Result<ElevationProfile>> FindOrCreate(ElevationProfile profile);

}
