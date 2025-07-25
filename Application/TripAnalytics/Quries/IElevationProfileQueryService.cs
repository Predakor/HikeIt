using Application.Dto.Analytics;
using Application.Interfaces;
using Domain.Common.Result;
using Domain.Trips.Config;

namespace Application.TripAnalytics.Quries;

public interface IElevationProfileQueryService : IQueryService {
    Task<Result<ElevationProfileDto>> GetElevationProfile(Guid id);
    Task<Result<ElevationProfileDto?>> DevAnalyticPreview(Guid id, ConfigBase.Nullable config);
}
