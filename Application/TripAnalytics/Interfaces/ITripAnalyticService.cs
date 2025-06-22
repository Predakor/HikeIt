using Application.Dto;
using Domain.Common.Result;
using Domain.Entiites.Users;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.Trips;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics.Interfaces;

public interface ITripAnalyticService {
    Task<TripAnalytic?> GetAnalytic(Guid id);
    Task<Result<TripAnalyticsDto.Full>> GetCompleteAnalytic(Guid id);
    Task<Result<TripAnalytic>> GenerateAnalytic(AnalyticData data, Trip trip, User user);
    Task<ElevationProfile> GetElevationProfile(Guid id);
}
