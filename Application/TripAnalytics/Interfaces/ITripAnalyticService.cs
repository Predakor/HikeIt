using Application.Dto;
using Application.Trips;
using Domain.Common.Result;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Entities.ElevationProfile;

namespace Application.TripAnalytics.Interfaces;

public interface ITripAnalyticService {
    Task<Result<TripAnalytic>> GetAnalytic(Guid id);
    Task<Result<TripAnalyticsDto.Full>> GetCompleteAnalytic(Guid id);
    Task<Result<TripAnalytic>> GenerateAnalytic(CreateTripContext ctx);
    Task<ElevationProfile> GetElevationProfile(Guid id);
}
