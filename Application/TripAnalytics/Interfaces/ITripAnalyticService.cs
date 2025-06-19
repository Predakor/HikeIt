using Domain.Common.Result;
using Domain.Entiites.Users;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.Trips;
using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics.Interfaces;

public interface ITripAnalyticService {
    Task<TripAnalytic?> GetAnalytic(Guid id);
    Task<Result<TripAnalytic>> GetCompleteAnalytic(Guid id);
    Task<Result<TripAnalytic>> GenerateAnalytic(AnalyticData data, Trip trip, User user);
    Task<ElevationProfile> CreateElevationProfile(ElevationDataWithConfig dataWithConfig);
    Task<ElevationProfile> GetElevationProfile(Guid id);
}