using Application.Dto.Analytics;
using Application.Interfaces;
using Domain.Common.Result;
using Domain.Trips.Config;

namespace Application.TripAnalytics.ElevationProfiles;

public interface IElevationProfileQueryService : IQueryService {
    Task<Result<ElevationProfileDto>> GetElevationProfile(Guid id);
    Task<Result<ElevationProfileDto?>> DevAnalyticPreview(Guid id, DataProccesConfig.Partial config);
}
