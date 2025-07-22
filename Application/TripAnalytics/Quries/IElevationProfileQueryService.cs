using Application.Dto.Analytics;
using Domain.Common.Result;
using Domain.Trips.Config;

namespace Application.TripAnalytics.Quries;

public interface IElevationProfileQueryService {
    Task<Result<ElevationProfileDto>> GetElevationProfile(Guid id);
    Task<Result<ElevationProfileDto?>> DevAnalyticPreview(Guid id, ConfigBase.Nullable config);
}
