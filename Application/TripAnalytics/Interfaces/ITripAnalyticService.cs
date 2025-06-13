using Domain.Common;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics.Interfaces;

public interface ITripAnalyticService {
    Task<TripAnalytic> GenerateAnalytic(AnalyticData data);
    Task<TripAnalytic?> GetAnalytic(Guid id);
    Task<Result<TripAnalytic>> GetCompleteAnalytic(Guid id);
    Task<ElevationProfile> GetElevationProfile(Guid id);
}