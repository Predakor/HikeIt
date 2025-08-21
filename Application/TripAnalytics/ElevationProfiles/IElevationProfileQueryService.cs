using Application.Commons.Interfaces;
using Application.Dto.Analytics;
using Domain.Common.Result;
using Domain.Trips.Config;

namespace Application.TripAnalytics.ElevationProfiles;

public interface IElevationProfileQueryService : IQueryService {
    Task<Result<ElevationProfileDto>> GetElevationProfile(Guid id);
    Task<Result<ElevationProfileDto?>> DevAnalyticPreview(Guid id, DataProccesConfig.Partial config);
}
