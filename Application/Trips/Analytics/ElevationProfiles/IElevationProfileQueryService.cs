using Application.Commons.Abstractions.Queries;
using Domain.Common.Result;
using Domain.Trips.Root.Builders.Config;

namespace Application.Trips.Analytics.ElevationProfiles;

public interface IElevationProfileQueryService : IQueryService {
    Task<Result<ElevationProfileDto>> GetElevationProfile(Guid id);
    Task<Result<ElevationProfileDto?>> DevAnalyticPreview(Guid id, DataProccesConfig.Partial config);
}
